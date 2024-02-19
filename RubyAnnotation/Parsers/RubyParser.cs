using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Syntax;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Utils;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Parsers;

/// <summary>
/// The inline parser used to for ruby annotations.
/// </summary>
/// <seealso cref="InlineParser" />
public class RubyParser : InlineParser
{
    public RubyParser()
    {
        OpeningCharacters = ['|', '｜', '(', '（', '《', '\\'];
    }

    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        switch (slice.CurrentChar)
        {
            case '\\':
                return ParseEscape(processor, ref slice);
            case '|' or '｜':
                return ParseRubyStart(processor, ref slice);
            case '(' or '（' or '《':
                return ParseRubyText(processor, ref slice);
        }

        return false;
    }

    private bool ParseEscape(InlineProcessor processor, ref StringSlice slice)
    {
        var start = processor.GetSourcePosition(slice.Start, out var line, out var column);
        if (slice.NextChar() is '｜' or '《' or '》' or '（' or '）')
        {
            processor.Inline = new LiteralInline()
            {
                Content = new(slice.Text, slice.Start, slice.Start),
                Span = new(start, start + 1),
                Line = line,
                Column = column,
                IsFirstCharacterEscaped = true,
            };
            slice.SkipChar();

            return true;
        }

        return false;
    }

    private bool ParseRubyStart(InlineProcessor processor, ref StringSlice slice)
    {
        if (processor.Inline?.FirstParentOfType<RubyInline>() is not null)
        {
            return false;
        }

        var start = processor.GetSourcePosition(slice.Start, out var line, out var column);

        var nc = slice.NextChar();
        if (nc is '(' or '（' or '《')
        {
            // 括弧の直前にパイプ文字がある場合はルビではなく普通の括弧として扱う（エスケープ記法）
            return true;
        }

        var rtStart = slice.FindWithoutEscaped(['(', '（', '《']);
        var rtEnd = slice.FindWithoutEscaped(slice.PeekCharAbsolute(rtStart) switch { '(' => ')', '（' => '）', '《' or _ => '》' }, rtStart);
        var delimiter = slice.FindWithoutEscaped(['|', '｜']);
        if (rtStart >= 0 && rtStart < rtEnd && (delimiter < 0 || delimiter > rtStart))
        {
            processor.Inline = new RubyInline
            {
                Line = line,
                Column = column,
                Span = new(start, start),
                Delimiter = new(slice.Text, slice.Start - 1, slice.Start - 1),
            };

            return true;
        }

        return false;
    }

    private int CountLeftKanji(in StringSlice slice)
    {
        var peek = 0;
        var vs = 0;
        while (true)
        {
            var pc = slice.PeekCharExtra(--peek);
            var code = (int)pc;
            if (pc is >= '\uDC00' and <= '\uDFFF' && slice.PeekCharExtra(peek - 1) is >= '\uD800' and <= '\uDBFF' and var hs)
            {
                // サロゲートペアはコードポイントに変換
                code = char.ConvertToUtf32(hs, pc);
                --peek;
            }

            // SVS異体字セレクタ
            if (vs == 0 && code is >= 0xFE00 and <= 0xFE0F)
            {
                vs = 1;
            }
            // IVS異体字セレクタ
            else if (vs == 0 && code is >= 0xE0100 and <= 0xE01EF)
            {
                vs = 2;
            }
            // 自動ルビの親文字はCJK漢字と「々」「〆」「〇」「〻」「ヶ」を対象とする
            // CJK漢字は追加面のものも含む
            else if (code is
                >= 0x4E00 and <= 0x9FFF or   // CJK統合漢字
                >= 0xF900 and <= 0xFAFF or   // CJK互換漢字
                >= 0x3400 and <= 0x4DBF or   // CJK統合漢字拡張A
                >= 0x20000 and <= 0x2A6DF or // CJK統合漢字拡張B
                >= 0x2A700 and <= 0x2B73F or // CJK統合漢字拡張C
                >= 0x2B740 and <= 0x2B81F or // CJK統合漢字拡張D
                >= 0x2B820 and <= 0x2CEAF or // CJK統合漢字拡張E
                >= 0x2CEB0 and <= 0x2EBEF or // CJK統合漢字拡張F
                >= 0x30000 and <= 0x3134F or // CJK統合漢字拡張G
                >= 0x31350 and <= 0x323AF or // CJK統合漢字拡張H
                >= 0x2EBF0 and <= 0x2EE5F or // CJK統合漢字拡張I
                >= 0x2F800 and <= 0x2FA1F or // CJK互換漢字補助
                '々' or '〆' or '〇' or '〻' or 'ヶ')
            {
                vs = 0;
            }
            else
            {
                // 結果の文字数はchar単位で返す
                return (code >= 0x10000 ? -2 : -1) - peek - vs;
            }
        }
    }

    private RubyInline? ParseRubyBase(InlineProcessor processor, ref StringSlice slice)
    {
        var count = CountLeftKanji(slice);

        if (count <= 0)
        {
            // 直前の文字が漢字でなければルビ化しない
            return null;
        }

        var start = processor.GetSourcePosition(slice.Start, out var line, out var column);
        var ruby = new RubyInline
        {
            Line = line,
            Column = column,
            Span = new(start - count, start),
            IsClosed = false,
        };

        if (processor.Inline is LiteralInline prevLiteral)
        {
            if (prevLiteral.Content.Length == count)
            {
                prevLiteral.ReplaceBy(ruby);
            }
            else
            {
                prevLiteral.Span.End -= count;
                prevLiteral.Content.End -= count;
                prevLiteral.InsertAfter(ruby);
            }
        }

        ruby.AppendChild(new LiteralInline
        {
            Line = line,
            Column = column,
            Span = new(start - count, start - 1),
            Content = new(slice.Text, slice.Start - count, slice.Start - 1),
            IsClosed = true,
        });

        return ruby;
    }

    private bool ParseRubyText(InlineProcessor processor, ref StringSlice slice)
    {
        var rtEnd = slice.FindWithoutEscaped(slice.CurrentChar switch { '(' => ')', '（' => '）', '《' or _ => '》' });
        if (rtEnd >= 0)
        {
            var ruby = processor.Inline?.FirstParentOfType<RubyInline>();
            var content = new StringSlice(slice.Text, slice.Start + 1, rtEnd - 1);

            if (ruby is null)
            {
                // ルビベースの始点が明示されていない場合、
                // 《》内がひらがなとカタカナのみであれば手前の漢字部分をルビベースとする

                if (slice.CurrentChar is not '《')
                {
                    // 《》以外の括弧は始点の明示を必須とする
                    return false;
                }

                if ((ruby = ParseRubyBase(processor, ref slice)) is null)
                {
                    return false;
                }
            }

            var start = processor.GetSourcePosition(slice.Start, out var line, out var column);
            processor.Inline = new RubyTextInline
            {
                Line = line,
                Column = column,
                Span = new(start, processor.GetSourcePosition(rtEnd)),
                Content = content,
                IsClosed = true,
            };
            slice.Start = rtEnd + 1;

            ruby.Span.End = processor.Inline.Span.End;
            ruby.IsClosed = true;
            ruby.HasRubyText = true;
            processor.PostProcessInlines(0, ruby, null, false);
            ruby.AppendChild(processor.Inline);

            return true;
        }

        return false;
    }
}