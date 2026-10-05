using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests.Attributes;

public class CliValueGroupTests
{
    [Test]
    public async Task Repeated_Groups_Preserve_Values_And_Occurrences()
    {
        var options = new ValueGroupOptions
        {
            Identities = [new(["operator", "identity one"]), new(["another", "identity two"])],
        };
        await Assert.That(BuildArguments(options)).IsEquivalentTo(
            ["--identity", "operator", "identity one", "--identity", "another", "identity two"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Optional_Groups_Render_Bare_And_Valued_Occurrences()
    {
        var options = new OptionalValueGroupOptions { Values = [new([]), new(["first", "second"])] };
        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--value", "--value", "first", "second"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Required_Group_Rejects_Empty_Occurrence()
    {
        await Assert.That(() => BuildArguments(new ValueGroupOptions { Identities = [new([])] }))
            .Throws<InvalidOperationException>().And.HasMessageContaining("non-empty value group");
    }

    [Test]
    public async Task Null_Groups_Are_Rejected()
    {
        await Assert.That(() => BuildArguments(new ValueGroupOptions { Identities = [null!] }))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Constructor_Snapshots_And_Rejects_Null_Values()
    {
        var values = new[] { "original" };
        var group = new CliValueGroup(values);
        values[0] = "changed";
        await Assert.That(group.Values).IsEquivalentTo(["original"]);
        await Assert.That(() => new CliValueGroup(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => new CliValueGroup([null!])).Throws<ArgumentException>();
    }

    [Test]
    [Arguments(OptionFormat.EqualsSeparated, true, null)]
    [Arguments(OptionFormat.SpaceSeparated, false, null)]
    [Arguments(OptionFormat.SpaceSeparated, true, ",")]
    public async Task Groups_Reject_Conflicting_Formatting(OptionFormat format, bool groupValues, string? separator)
    {
        var part = new OptionPart("Values", _ => new CliValueGroup(["value"]),
            new CliOptionAttribute("--value") { Format = format, GroupValues = groupValues, CollectionSeparator = separator });
        await Assert.That(() => new CommandArgumentBuilder().BuildArguments([part], new object()))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Empty_Collection_Omits_Option()
    {
        await Assert.That(BuildArguments(new ValueGroupOptions { Identities = [] })).IsEmpty();
    }
}

[CliTool("tool")]
internal record ValueGroupOptions : CommandLineToolOptions
{
    [CliOption("--identity", GroupValues = true)]
    public IEnumerable<CliValueGroup>? Identities { get; init; }
}

[CliTool("tool")]
internal record OptionalValueGroupOptions : CommandLineToolOptions
{
    [CliOption("--value", GroupValues = true, ValueArity = CliOptionValueArity.Optional)]
    public IEnumerable<CliValueGroup>? Values { get; init; }
}
