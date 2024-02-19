# Ruby Annotation Markup Extension for Markdig

**[Japanese Version](https://github.com/noy-shimotsuki/TsukuyoOka.MarkdigExtensions/blob/main/RubyAnnotation/README-ja.md)**

This extension adds the ability to parse Aozora Bunko format ruby syntax to Markdig's Markdown parser, 
enabling the use of ruby annotations in Markdown.

## NuGet Package

https://www.nuget.org/packages/TsukuyoOka.MarkdigExtensions.RubyAnnotation/

```powershell
Install-Package TsukuyoOka.MarkdigExtensions.RubyAnnotation
```

## What is ruby?

Ruby is a short annotation that appears in small letters above or to the right of the text.
They are generally used in East Asia, especially Japan, to guide the pronunciation of kanji characters.

The text to be annotated is called the base text.

## How to use

Generate and use Markdig's `MarkdownPipeline` with `UseRuby()`.

```csharp
using Markdig;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation;

var pipeline = new MarkdownPipelineBuilder().UseRuby().Build();

Console.WriteLine(Markdown.ToHtml("漢字《かんじ》", pipeline));
// → <p><ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby></p>
Console.WriteLine(Markdown.ToPlainText("漢字《かんじ》", pipeline));
// → 漢字
```

You can also change the method of outputting ruby annotations.

```csharp
using Markdig;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation;

var pipeline = new MarkdownPipelineBuilder()
    .UseRuby(new()
    {
        EnableRubyRenderToPlainText = true,
        OpenParenthesisForRendering = "〔",
        CloseParenthesisForRendering = "〕"
    })
    .Build();

Console.WriteLine(Markdown.ToHtml("漢字《かんじ》", pipeline));
// → <p><ruby>漢字<rp>〔</rp><rt>かんじ</rt><rp>〕</rp></ruby></p>
Console.WriteLine(Markdown.ToPlainText("漢字《かんじ》", pipeline));
// → 漢字〔かんじ〕
```

## Syntax

The syntax in this extension is based on the ruby markup syntax adopted by [Aozora Bunko (lang: ja)](https://www.aozora.gr.jp/) and now widely used in Japan,
especially on novel submission sites such as [Shōsetsuka ni Narō (lang: ja)](https://syosetu.com/).

### Kanji《ruby text》 Syntax

In this syntax, the ruby text is written immediately after the base text, enclosed in double angle brackets `《`<sub>(U+300A)</sub> `》`<sub>(U+300B)</sub>. 
The base text is kanji characters immediately before the opening bracket.

#### Examples

* `漢字《かんじ》` → <ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby>
* `隴西《ろうせい》の李徴《りちょう》` → <ruby>隴西<rp>(</rp><rt>ろうせい</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>
* `霞ヶ浦《かすみがうら》` → <ruby>霞ヶ浦<rp>(</rp><rt>かすみがうら</rt><rp>)</rp></ruby>
* `代々木《よよぎ》` → <ruby>代々木<rp>(</rp><rt>よよぎ</rt><rp>)</rp></ruby>
* `本音《建前》` → <ruby>本音<rp>(</rp><rt>建前</rt><rp>)</rp></ruby>
* `Hello《こんにちは》` → Hello《こんにちは》

### ｜Base text《ruby text》 Syntax

In this syntax, a delimiter `｜`<sub>(U+FF5C)</sub> is written at the beginning of the base text,
and the ruby text is written immediately after the base text, enclosed in double angle brackets `《`<sub>(U+300A)</sub> `》`<sub>(U+300B)</sub>.

The base text can contain both kanji and non-kanji characters.

ASCII `|`<sub>(U+007C)</sub> can be used instead of `｜`.

#### Examples

* `博学｜才穎《さいえい》` → 博学<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby>
* `｜Hello《こんにちは》` → <ruby>Hello<rp>(</rp><rt>こんにちは</rt><rp>)</rp></ruby>
* `｜極・雷斬光剣《アルティメット　ライジングスラッシュ》` → <ruby>極・雷斬光剣<rp>(</rp><rt>アルティメット　ライジングスラッシュ</rt><rp>)</rp></ruby>

### |Base text (ruby text) Syntax

The original Aozora Bunko format uses non-ASCII full-width characters,
but this extension also defines a syntax that allows writing in ASCII only.

In this syntax, even if the base text is only kanji characters, the range must be indicated by `|`<sub>(U+007C)</sub>.

Although the syntax is defined for ASCII, not only ASCII `|`<sub>(U+007C)</sub> `(`<sub>(U+0028)</sub> `)`<sub>(U+0029)</sub>
but also full-width `｜`<sub>(U+FF5C)</sub> `（`<sub>(U+FF08)</sub> `）`<sub>(U+FF09)</sub> can be used.

#### Examples

* `|漢字(かんじ)` → <ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby>
* `|隴西(ろうせい)の|李徴(りちょう)` → <ruby>隴西<rp>(</rp><rt>ろうせい</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>
* `漢字(かんじ)` → 漢字(かんじ)

### Escape Syntax

If you want to use as normal text instead of a ruby syntax, insert `｜`<sub>(U+FF5C)</sub> or ASCII `|`<sub>(U+007C)</sub> immediately before the opening bracket.
Otherwise, the standard Markdown escape character using `\`<sub>(U+005C)</sub> will also work.

#### Examples

* `漢字｜《かんじ》` → 漢字《かんじ》
* `漢字\《かんじ》` → 漢字《かんじ》
* `漢字《かんじ\》` → 漢字《かんじ》
* `漢字《かんじ\》かんじ》` → <ruby>漢字<rp>(</rp><rt>かんじ》かんじ</rt><rp>)</rp></ruby>
* `\｜ハロー《こんにちは》` → ｜ハロー《こんにちは》
* `ハロー|(こんにちは)` → ハロー(こんにちは)

### Definition of kanji characters

The term "kanji" in this extension refers to the following in Unicode.
Kanji characters include those used only in Japan, those used only in China,
those common to all kanji cultures sphere, etc., but no distinction is made.

For each symbol that is not included in Kanji but treated as equivalent to Kanji,
we follow the [Aozora Bunko's Guideline (lang: ja)](https://www.aozora.gr.jp/aozora-manual/index-input.html#ruby).

* CJK Unified ideographs (U+4E00 - U+9FFF)
* CJK Compatibility Ideographs (U+F900 - U+FAFF)
* CJK Unified Ideographs Extension A (U+3400 - U+4DBF)
* CJK Unified Ideographs Extension B (U+20000 - U+2A6DF)
* CJK Unified Ideographs Extension C (U+2A700 - U+2B73F)
* CJK Unified Ideographs Extension D (U+2B740 - U+2B81F)
* CJK Unified Ideographs Extension E (U+2B820 - U+2CEAF)
* CJK Unified Ideographs Extension F (U+2CEB0 - U+2EBEF)
* CJK Unified Ideographs Extension G (U+30000 - U+3134F)
* CJK Unified Ideographs Extension H (U+31350 - U+323AF)
* CJK Unified Ideographs Extension I (U+2EBF0 - U+2EE5F)
* CJK Compatibility Ideographs Supplement (U+2F800 - U+2FA1F)
* Ideographic Iteration Mark (同の字点; Dō no jiten) "々" (U+3005)
* Ideographic Closing Mark (しめ; Shime) "〆" (U+3006)
* Ideographic Number Zero "〇" (U+3007)
* Vertical Ideographic Iteration Mark (二の字点; Ni no jiten) "〻" (U+303B)
* Katakana Letter Small KE "ヶ" (U+30F6)

It also supports the variation selectors of both the Standardized Variation Sequence (SVS) and the Ideographic Variation Sequence (IVS).

* Variation Selectors of the SVS (U+FE00 - U+FE0F)
* Variation Selectors of the IVS (U+E0100 - U+E01EF)

If a variant selector is immediately follows a kanji character, it is treated as part of the kanji character.
Conversely, if it follows a non-kanji character, it is treated as part of the non-kanji character.

It does not determine whether the combination of the kanji character and the variant selector is correct as defined in the Unicode specification or the Ideographic Variation Database (IVD).
