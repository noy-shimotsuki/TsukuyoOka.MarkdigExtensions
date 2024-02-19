# ルビマークアップ拡張 for Markdig

**[English Version](https://github.com/noy-shimotsuki/TsukuyoOka.MarkdigExtensions/blob/main/RubyAnnotation/README.md)**

MarkdigのMarkdownパーサーに青空文庫形式のルビ構文をパースする機能を追加し、Markdownでルビを振れるようにします。

## NuGetパッケージ

https://www.nuget.org/packages/TsukuyoOka.MarkdigExtensions.RubyAnnotation/

```powershell
Install-Package TsukuyoOka.MarkdigExtensions.RubyAnnotation
```

## 使用方法

Markdigの`MarkdownPipeline`を`UseRuby()`付きで生成して使用します。

```csharp
using Markdig;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation;

var pipeline = new MarkdownPipelineBuilder().UseRuby().Build();

Console.WriteLine(Markdown.ToHtml("漢字《かんじ》", pipeline));
// → <p><ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby></p>
Console.WriteLine(Markdown.ToPlainText("漢字《かんじ》", pipeline));
// → 漢字
```

ルビ生成のオプションを指定することも出来ます。現時点では主にプレーンテキストとして出力する場合に使用します。

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

## 構文

[青空文庫](https://www.aozora.gr.jp/)で採用され、現在では[小説家になろう](https://syosetu.com/)など小説投稿サイトを中心に日本で広く利用されているルビマークアップ構文をベースにしています。

### 漢字《ルビ》方式

ルビを振りたい漢字の直後に二重山括弧 `《`<sub>(U+300A)</sub> `》`<sub>(U+300B)</sub> で囲って記述します。
この場合、漢字以外の文字にルビを振ることは出来ません。

#### 使用例

* `漢字《かんじ》` → <ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby>
* `隴西《ろうせい》の李徴《りちょう》` → <ruby>隴西<rp>(</rp><rt>ろうせい</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>
* `霞ヶ浦《かすみがうら》` → <ruby>霞ヶ浦<rp>(</rp><rt>かすみがうら</rt><rp>)</rp></ruby>
* `代々木《よよぎ》` → <ruby>代々木<rp>(</rp><rt>よよぎ</rt><rp>)</rp></ruby>
* `本音《建前》` → <ruby>本音<rp>(</rp><rt>建前</rt><rp>)</rp></ruby>
* `ハロー《こんにちは》` → ハロー《こんにちは》

### ｜親文字《ルビ》方式

ルビを振りたい文字の直前に `｜`<sub>(U+FF5C)</sub> を記述し、ルビを振りたい文字の直後に二重山括弧 `《`<sub>(U+300A)</sub> `》`<sub>(U+300B)</sub> で囲って記述します。
漢字以外の文字にルビを振りたい、あるいは漢字列の途中からルビを振りたい場合はこちらを使用します。

`｜` の代わりに半角 `|`<sub>(U+007C)</sub> も使用可能です。

#### 使用例

* `博学｜才穎《さいえい》` → 博学<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby>
* `｜ハロー《こんにちは》` → <ruby>ハロー<rp>(</rp><rt>こんにちは</rt><rp>)</rp></ruby>
* `｜極・雷斬光剣《アルティメット　ライジングスラッシュ》` → <ruby>極・雷斬光剣<rp>(</rp><rt>アルティメット　ライジングスラッシュ</rt><rp>)</rp></ruby>

### |親文字(ルビ)方式

Markdownの他の記法に合わせ、ASCIIのみでルビを振れる構文を用意しています。
ASCIIの `|`<sub>(U+007C)</sub> `(`<sub>(U+0028)</sub> `)`<sub>(U+0029)</sub> だけでなく、全角の `｜`<sub>(U+FF5C)</sub> `（`<sub>(U+FF08)</sub> `）`<sub>(U+FF09)</sub> も使用可能です。

ただしこちらは `|` が必須です。 `《` `》` とは異なり、 `(` `)` だけでルビを振ることはできません。

#### 使用例

* `|漢字(かんじ)` → <ruby>漢字<rp>(</rp><rt>かんじ</rt><rp>)</rp></ruby>
* `|隴西(ろうせい)の|李徴(りちょう)` → <ruby>隴西<rp>(</rp><rt>ろうせい</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>
* `漢字(かんじ)` → 漢字(かんじ)

### エスケープ記法

`《`<sub>(U+300A)</sub> `》`<sub>(U+300B)</sub> をルビではなく通常の文字として使用したい場合、 `《` の直前に `｜`<sub>(U+FF5C)</sub> （半角 `|`<sub>(U+007C)</sub> も可）を記述します。
その他、`\`<sub>(U+005C)</sub> を使用したMarkdown標準のエスケープ記法も有効です。

#### 使用例

* `漢字｜《かんじ》` → 漢字《かんじ》
* `漢字\《かんじ》` → 漢字《かんじ》
* `漢字《かんじ\》` → 漢字《かんじ》
* `漢字《かんじ\》かんじ》` → <ruby>漢字<rp>(</rp><rt>かんじ》かんじ</rt><rp>)</rp></ruby>
* `\｜ハロー《こんにちは》` → ｜ハロー《こんにちは》
* `ハロー|(こんにちは)` → ハロー(こんにちは)

### 漢字の範囲について

この拡張での「漢字」とは、Unicodeにおいて以下に該当するものを指します。日本語では使用しない漢字も多く含まれますが、特に区別しません。

漢字には含まれないが漢字相当として扱う各記号については[青空文庫の指針](https://www.aozora.gr.jp/aozora-manual/index-input.html#ruby)に準拠しています。

* CJK統合漢字 (U+4E00 〜 U+9FFF)
* CJK互換漢字 (U+F900 〜 U+FAFF)
* CJK統合漢字拡張A (U+3400 〜 U+4DBF)
* CJK統合漢字拡張B (U+20000 〜 U+2A6DF)
* CJK統合漢字拡張C (U+2A700 〜 U+2B73F)
* CJK統合漢字拡張D (U+2B740 〜 U+2B81F)
* CJK統合漢字拡張E (U+2B820 〜 U+2CEAF)
* CJK統合漢字拡張F (U+2CEB0 〜 U+2EBEF)
* CJK統合漢字拡張G (U+30000 〜 U+3134F)
* CJK統合漢字拡張H (U+31350 〜 U+323AF)
* CJK統合漢字拡張I (U+2EBF0 〜 U+2EE5F)
* CJK互換漢字補助 (U+2F800 〜 U+2FA1F)
* 同の字点「々」 (U+3005)
* しめ「〆」 (U+3006)
* 漢数字のゼロ「〇」 (U+3007)
* 二の字点「〻」 (U+303B)
* 「ヶ」 (U+30F6)

また、標準化異体字シーケンス（SVS）と漢字異体字シーケンス（IVS）の異体字セレクタに対応しています。

* SVS用の異体字セレクタ (U+FE00 〜 U+FE0F)
* IVS用の異体字セレクタ (U+E0100 〜 U+E01EF)

漢字の直後に異体字セレクタが1つ続く場合、それも含めて漢字として扱い、ルビを振ります。
漢字以外に続く場合は漢字として扱いません。

漢字と異体字セレクタの組み合わせが正しい（Unicode仕様や漢字異体字データベース〈IVD〉に定義されている）ものかどうかは判断しません。
