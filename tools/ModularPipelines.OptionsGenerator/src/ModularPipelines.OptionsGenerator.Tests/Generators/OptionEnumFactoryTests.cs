using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class OptionEnumFactoryTests
{
    [Test]
    public async Task Preserves_Literal_Values_And_Documentation_With_Unique_Identifiers()
    {
        var values = new (string Value, string? Description)[]
        {
            ("foo-bar", "Primary spelling."),
            ("foo_bar", "Alternative spelling."),
            ("FOO_BAR", "Uppercase spelling."),
            ("1st", "Leading digit."),
            ("net8.0", "Contains punctuation."),
            ("+", "Symbol."),
            ("foo-bar", "Additional documentation."),
            ("foo-bar", "Primary spelling."),
        };

        var definition = OptionEnumFactory.TryCreate("TestOptions", "Mode", "--mode", values)!;
        var reordered = OptionEnumFactory.TryCreate("TestOptions", "Mode", "--mode", values.Reverse())!;

        await Assert.That(definition.Values.Select(value => value.CliValue))
            .IsEquivalentTo(values.Select(value => value.Value).Distinct(StringComparer.Ordinal));
        await Assert.That(definition.Values.Select(value => value.MemberName).Distinct().Count())
            .IsEqualTo(definition.Values.Count);
        await Assert.That(definition.Values.All(value => SyntaxFacts.IsValidIdentifier(value.MemberName))).IsTrue();
        await Assert.That(definition.Values.Single(value => value.CliValue == "foo-bar").Description)
            .IsEqualTo("Additional documentation. Primary spelling.");
        await Assert.That(definition.Values).IsEquivalentTo(reordered.Values);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, true)]
    [Arguments(20, true)]
    [Arguments(21, false)]
    public async Task Member_Limit_Counts_Distinct_Cli_Values(int count, bool expectEnum)
    {
        var values = Enumerable.Range(1, count).Select(index => $"value{index}").ToArray();
        var definition = OptionEnumFactory.TryCreate("TestOptions", "Mode", "--mode", values.Concat(values));

        await Assert.That(definition is not null).IsEqualTo(expectEnum);
        if (expectEnum)
        {
            await Assert.That(definition!.Values).Count().IsEqualTo(count);
        }
    }
}
