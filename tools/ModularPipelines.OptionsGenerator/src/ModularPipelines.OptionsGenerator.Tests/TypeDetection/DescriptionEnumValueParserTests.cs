using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class DescriptionEnumValueParserTests
{
    [Test]
    public async Task TryParse_Rejects_Explanatory_Parenthesized_Lists()
    {
        var result = DescriptionEnumValueParser.TryParse("Size (bytes, kilobytes, or megabytes)");

        await Assert.That(result).IsNull();
    }

    [Test]
    [Arguments("AWS OIDC: replace the session policy ARN list (repeatable, comma-separated)")]
    [Arguments("GitHub: replace the path filter list (repeatable, comma-separated)")]
    [Arguments("Delete an environment variable by key (repeatable, comma-separated)")]
    public async Task TryParse_Rejects_Repeatability_Annotations(string description)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNull();
    }

    [Test]
    [Arguments("Output format (table, json, yaml)", "table,json,yaml")]
    [Arguments("Log types to enable (all, none, api, audit)", "all,none,api,audit")]
    [Arguments("Specify authentication window preference (silent, silentPreferred, or interactive)", "silent,silentPreferred,interactive")]
    [Arguments("""Set the logging level ("debug", "info", "warn", "error", "fatal")""", "debug,info,warn,error,fatal")]
    public async Task TryParse_Accepts_Contextual_Parenthesized_Values(string description, string expectedValues)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values).IsEquivalentTo(expectedValues.Split(','));
        await Assert.That(result.MatchKind).IsEqualTo(DescriptionEnumMatchKind.ContextualParenthesized);
    }

    [Test]
    public async Task TryParse_Preserves_Trailing_Value_Before_Sentence_Punctuation()
    {
        var result = DescriptionEnumValueParser.TryParse("Valid values: json, yaml, xml.");

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values).IsEquivalentTo(new[] { "json", "yaml", "xml" });
    }

    [Test]
    [Arguments("Valid values: json, linux/amd64, yaml")]
    [Arguments("Valid values: json, invalid@value, yaml")]
    public async Task TryParse_Rejects_Entire_List_When_Any_Value_Is_Unsafe(string description)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task TryParse_Accepts_Explicit_OneOf_Values()
    {
        var result = DescriptionEnumValueParser.TryParse("Must be one of: table|json|yaml");

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values).IsEquivalentTo(new[] { "table", "json", "yaml" });
        await Assert.That(result.MatchKind).IsEqualTo(DescriptionEnumMatchKind.Explicit);
    }

    [Test]
    [Arguments("One of: json or yaml", "json,yaml")]
    [Arguments("Optionally specify one of 'url', 'file', 'release' or 'source'", "url,file,release,source")]
    [Arguments("""Must be "background", "orphan", or "foreground".""", "background,orphan,foreground")]
    public async Task TryParse_Accepts_Prose_Separated_Explicit_Values(
        string description,
        string expectedValues)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values).IsEquivalentTo(expectedValues.Split(','));
        await Assert.That(result.MatchKind).IsEqualTo(DescriptionEnumMatchKind.Explicit);
    }

    [Test]
    [Arguments("Valid values are low and high")]
    [Arguments("list of directories; the configured path must be one of them, accepts comma-separated values")]
    public async Task TryParse_Rejects_Ordinary_Prose(string description)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNull();
    }

    [Test]
    [Arguments("Output format. One of: (json, yaml, xml)", "json,yaml,xml")]
    [Arguments("Accepted values: [json, yaml, xml].", "json,yaml,xml")]
    [Arguments("Compression (possible values: gzip, none)", "gzip,none")]
    public async Task TryParse_Accepts_Wrapped_Explicit_Values(string description, string expectedValues)
    {
        var result = DescriptionEnumValueParser.TryParse(description);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values).IsEquivalentTo(expectedValues.Split(','));
        await Assert.That(result.MatchKind).IsEqualTo(DescriptionEnumMatchKind.Explicit);
    }
}
