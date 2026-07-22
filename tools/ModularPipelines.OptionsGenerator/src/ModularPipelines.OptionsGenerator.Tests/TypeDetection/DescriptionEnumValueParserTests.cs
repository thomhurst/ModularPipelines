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
    [Arguments("Output format (table, json, yaml)", "table,json,yaml")]
    [Arguments("Log types to enable (all, none, api, audit)", "all,none,api,audit")]
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
}
