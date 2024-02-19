using TsukuyoOka.MarkdigExtensions.RubyAnnotation;

namespace RubyAnnotation.Tests;

public class RubyTest
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseRuby().Build();

    [Theory]
    // 括弧のパターン
    [InlineData(
        "隴西《ろうせい》の李徴《りちょう》は博学｜才穎《さいえい》",
        "<p><ruby>隴西<rp>(</rp><rt>ろうせい</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>は博学<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby></p>\n")]
    [InlineData(
        "隴西(ろうせい)の李徴(りちょう)は博学|才穎(さいえい)",
        "<p>隴西(ろうせい)の李徴(りちょう)は博学<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby></p>\n")]
    [InlineData(
        "隴西（ろうせい）の李徴（りちょう）は博学｜才穎（さいえい）",
        "<p>隴西（ろうせい）の李徴（りちょう）は博学<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby></p>\n")]
    // エスケープ
    [InlineData(
        "隴西\\《ろうせい》の李徴|《りちょう》は博学\\｜才穎《さいえい》",
        "<p>隴西《ろうせい》の李徴《りちょう》は博学｜<ruby>才穎<rp>(</rp><rt>さいえい</rt><rp>)</rp></ruby></p>\n")]
    [InlineData(
        "隴西｜《ろうせい》の｜李\\|\\｜徴《り\\》ちょう》は博学｜才穎《さいえい\\》",
        "<p>隴西《ろうせい》の<ruby>李|｜徴<rp>(</rp><rt>り》ちょう</rt><rp>)</rp></ruby>は博学｜才穎《さいえい》</p>\n")]
    [InlineData("｜李徴\\《りちょう》《りちょう》", "<p><ruby>李徴《りちょう》<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></p>\n")]
    // ルビテキストの文字種
    [InlineData(
        "隴西《ロウセイ》の李徴《リチョウ》は博学｜才穎《サイエイ》",
        "<p><ruby>隴西<rp>(</rp><rt>ロウセイ</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>リチョウ</rt><rp>)</rp></ruby>は博学<ruby>才穎<rp>(</rp><rt>サイエイ</rt><rp>)</rp></ruby></p>\n")]
    [InlineData(
        "隴西《ﾛｳｾｲ》の李徴《ﾘﾁｮｳ》は博学｜才穎《ｻｲｴｲ》",
        "<p><ruby>隴西<rp>(</rp><rt>ﾛｳｾｲ</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>ﾘﾁｮｳ</rt><rp>)</rp></ruby>は博学<ruby>才穎<rp>(</rp><rt>ｻｲｴｲ</rt><rp>)</rp></ruby></p>\n")]
    [InlineData(
        "隴西《rousei》の李徴《ｒｉｃｈｏｕ》は博学｜才穎《saiei》",
        "<p><ruby>隴西<rp>(</rp><rt>rousei</rt><rp>)</rp></ruby>の<ruby>李徴<rp>(</rp><rt>ｒｉｃｈｏｕ</rt><rp>)</rp></ruby>は博学<ruby>才穎<rp>(</rp><rt>saiei</rt><rp>)</rp></ruby></p>\n")]
    // 漢字のパターン
    [InlineData("染井𠮷野《そめいよしの》", "<p><ruby>染井𠮷野<rp>(</rp><rt>そめいよしの</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("㐂寿《きじゅ》", "<p><ruby>㐂寿<rp>(</rp><rt>きじゅ</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("山﨑《やまざき》", "<p><ruby>山﨑<rp>(</rp><rt>やまざき</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("益々《ますます》己《おのれ》の", "<p><ruby>益々<rp>(</rp><rt>ますます</rt><rp>)</rp></ruby><ruby>己<rp>(</rp><rt>おのれ</rt><rp>)</rp></ruby>の</p>\n")]
    [InlineData("益〻《ますます》己《おのれ》の", "<p><ruby>益〻<rp>(</rp><rt>ますます</rt><rp>)</rp></ruby><ruby>己<rp>(</rp><rt>おのれ</rt><rp>)</rp></ruby>の</p>\n")]
    [InlineData("💴壹〇〇萬圓《ひゃくまんえん》", "<p>💴<ruby>壹〇〇萬圓<rp>(</rp><rt>ひゃくまんえん</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("明日の〆切《しめきり》", "<p>明日の<ruby>〆切<rp>(</rp><rt>しめきり</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("霞ヶ浦《かすみがうら》", "<p><ruby>霞ヶ浦<rp>(</rp><rt>かすみがうら</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("葛飾区《かつしかく》、葛\udb40\udd00城市《かつらぎし》", "<p><ruby>葛飾区<rp>(</rp><rt>かつしかく</rt><rp>)</rp></ruby>、<ruby>葛\uDB40\uDD00城市<rp>(</rp><rt>かつらぎし</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("神\uFE00社\uFE00《じんじゃ》", "<p><ruby>神\uFE00社\uFE00<rp>(</rp><rt>じんじゃ</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("\U00030EDD\U00030EDD麺《ビャンビャンメン》", "<p><ruby>\U00030EDD\U00030EDD麺<rp>(</rp><rt>ビャンビャンメン</rt><rp>)</rp></ruby></p>\n")]
    // 非ルビ判定
    [InlineData("《ルビベースなし》", "<p>《ルビベースなし》</p>\n")]
    [InlineData("한글《ハングル》", "<p>한글《ハングル》</p>\n")]
    [InlineData("漢字《かんじ", "<p>漢字《かんじ</p>\n")]
    [InlineData("漢字《かんじ\\》", "<p>漢字《かんじ》</p>\n")]
    [InlineData("漢字《かんじ)", "<p>漢字《かんじ)</p>\n")]
    [InlineData("漢字（かんじ)", "<p>漢字（かんじ)</p>\n")]
    [InlineData("漢字（かんじ》", "<p>漢字（かんじ》</p>\n")]
    [InlineData("神\uFE00社\uFE00\uFE00《じんじゃ》", "<p>神\uFE00社\uFE00\uFE00《じんじゃ》</p>\n")]
    [InlineData("葛\uDB40\uDD00\uDB40\uDD00城市《かつらぎし》", "<p>葛\uDB40\uDD00\uDB40\uDD00<ruby>城市<rp>(</rp><rt>かつらぎし</rt><rp>)</rp></ruby></p>\n")]
    // 複合
    [InlineData("**李徴《りちょう》**", "<p><strong><ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></strong></p>\n")]
    [InlineData("|**李徴**《りちょう》", "<p><ruby><strong>李徴</strong><rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("**李徴**《りちょう》", "<p><strong>李徴</strong>《りちょう》</p>\n")]
    [InlineData("|**李徴《りちょう》**", "<p><ruby>**李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby>**</p>\n")]
    [InlineData("[李徴《りちょう》](https://www.example.com/)", "<p><a href=\"https://www.example.com/\"><ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></a></p>\n")]
    [InlineData("|[李徴](https://www.example.com/)《りちょう》", "<p><ruby><a href=\"https://www.example.com/\">李徴</a><rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("[李徴](https://www.example.com/)《りちょう》", "<p><a href=\"https://www.example.com/\">李徴</a>《りちょう》</p>\n")]
    [InlineData("|[李徴](https://www.example.com/)(りちょう)", "<p><ruby><a href=\"https://www.example.com/\">李徴</a><rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></p>\n")]
    [InlineData("|[李徴](https://www.example.com/)", "<p>|<a href=\"https://www.example.com/\">李徴</a></p>\n")]
    [InlineData("# 李徴《りちょう》", "<h1><ruby>李徴<rp>(</rp><rt>りちょう</rt><rp>)</rp></ruby></h1>\n")]
    public void TestHtmlRuby(string markdown, string html)
    {
        Assert.Equal(html, Markdown.ToHtml(markdown, _pipeline));
    }

    [Theory]
    [InlineData("李徴《りちょう》", "（", "）", "<p><ruby>李徴<rp>（</rp><rt>りちょう</rt><rp>）</rp></ruby></p>\n")]
    [InlineData("李徴《りちょう》", "＜", "＞", "<p><ruby>李徴<rp>＜</rp><rt>りちょう</rt><rp>＞</rp></ruby></p>\n")]
    public void TestHtmlRubyWithOptions(string markdown, string openParenthesis, string closeParenthesis, string html)
    {
        Assert.Equal(
            html,
            Markdown.ToHtml(markdown, new MarkdownPipelineBuilder()
                .UseRuby(new()
                {
                    OpenParenthesisForRendering = openParenthesis,
                    CloseParenthesisForRendering = closeParenthesis
                }).Build()));
    }

    [Theory]
    [InlineData(
        "隴西《ろうせい》の李徴《りちょう》は博学｜才穎《さいえい》",
        "隴西の李徴は博学才穎\n")]
    [InlineData(
        "隴西(ろうせい)の李徴(りちょう)は博学|才穎(さいえい)",
        "隴西(ろうせい)の李徴(りちょう)は博学才穎\n")]
    [InlineData(
        "隴西（ろうせい）の李徴（りちょう）は博学｜才穎（さいえい）",
        "隴西（ろうせい）の李徴（りちょう）は博学才穎\n")]
    public void TestPlainTextRuby(string markdown, string text)
    {
        Assert.Equal(text, Markdown.ToPlainText(markdown, _pipeline));
    }

    [Theory]
    [InlineData("李徴《りちょう》", false, null, null, "李徴\n")]
    [InlineData("李徴《りちょう》", true, null, null, "李徴(りちょう)\n")]
    [InlineData("李徴《りちょう》", true, "（", "）", "李徴（りちょう）\n")]
    [InlineData("李徴《りちょう》", true, "＜", "＞", "李徴＜りちょう＞\n")]
    [InlineData("李徴《り\\》ちょう》", true, "＜", "＞", "李徴＜り》ちょう＞\n")]
    public void TestPlainTextRubyWithOptions(string markdown, bool enableRuby, string? openParenthesis, string? closeParenthesis, string text)
    {
        if (openParenthesis is not null && closeParenthesis is not null)
        {
            Assert.Equal(
                text,
                Markdown.ToPlainText(markdown, new MarkdownPipelineBuilder()
                    .UseRuby(new()
                    {
                        EnableRubyRenderToPlainText = enableRuby,
                        OpenParenthesisForRendering = openParenthesis,
                        CloseParenthesisForRendering = closeParenthesis
                    }).Build()));
        }
        else
        {
            Assert.Equal(
                text,
                Markdown.ToPlainText(markdown, new MarkdownPipelineBuilder()
                    .UseRuby(new()
                    {
                        EnableRubyRenderToPlainText = enableRuby
                    }).Build()));
        }
    }
}