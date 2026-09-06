using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Models;

public class CliGlobalOptionMergerTests
{
    [Test]
    public async Task Merge_Combines_And_Orders_Scraped_And_Supplemental_Options()
    {
        var merged = CliGlobalOptionMerger.Merge(
            [Option("--zeta", "Zeta")],
            [Option("--alpha", "Alpha") with { Availability = "Enterprise" }]);

        await Assert.That(merged).Count().IsEqualTo(2);
        await Assert.That(merged[0].SwitchName).IsEqualTo("--alpha");
        await Assert.That(merged[1].SwitchName).IsEqualTo("--zeta");
        await Assert.That(merged[0].Availability).IsEqualTo("Enterprise");
    }

    [Test]
    public async Task Merge_Deduplicates_A_Compatible_Option_And_Augments_Documentation()
    {
        var scraped = Option("--license-key", "LicenseKey") with
        {
            Description = "License key from CLI help.",
            IsSecret = true,
        };
        var supplemental = scraped with
        {
            Description = "Supplemental description.",
            DocumentationUrl = "https://example.test/license",
            Availability = "Secure",
        };

        var merged = CliGlobalOptionMerger.Merge([scraped], [supplemental]);

        await Assert.That(merged).Count().IsEqualTo(1);
        await Assert.That(merged[0].Description).IsEqualTo("License key from CLI help.");
        await Assert.That(merged[0].DocumentationUrl).IsEqualTo("https://example.test/license");
        await Assert.That(merged[0].Availability).IsEqualTo("Secure");
    }

    [Test]
    public async Task Merge_Deduplicates_Structurally_Equivalent_Enum_Definitions()
    {
        var scraped = Option("--format", "Format") with
        {
            CSharpType = "OutputFormat?",
            EnumDefinition = EnumDefinition(),
        };
        var supplemental = scraped with
        {
            EnumDefinition = EnumDefinition(),
        };

        var merged = CliGlobalOptionMerger.Merge([scraped], [supplemental]);

        await Assert.That(merged).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Merge_Deduplicates_Enum_Definitions_Whose_Values_Were_Scraped_In_A_Different_Order()
    {
        var json = new CliEnumValue { MemberName = "Json", CliValue = "json" };
        var yaml = new CliEnumValue { MemberName = "Yaml", CliValue = "yaml" };
        var scraped = Option("--format", "Format") with
        {
            CSharpType = "OutputFormat?",
            EnumDefinition = EnumDefinition() with { Values = [yaml, json] },
        };
        var supplemental = scraped with
        {
            EnumDefinition = EnumDefinition() with { Values = [json, yaml] },
        };

        var merged = CliGlobalOptionMerger.Merge([scraped], [supplemental]);

        await Assert.That(merged).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Merge_Rejects_Conflicting_Definitions_For_The_Same_Switch()
    {
        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [Option("--output", "Output")],
                [Option("--output", "Output") with { CSharpType = "bool?", IsFlag = true }]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--output");
    }

    [Test]
    public async Task Merge_Rejects_Conflicting_GroupValues_For_The_Same_Switch()
    {
        var scraped = Option("--arguments", "Arguments") with { ValueSeparator = " " };

        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [scraped],
                [scraped with { GroupValues = true }]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--arguments");
    }

    [Test]
    public async Task Merge_Rejects_Conflicting_Rendering_Shape_For_The_Same_Switch()
    {
        var scraped = Option("--arguments", "Arguments");

        using (Assert.Multiple())
        {
            await Assert.That(() => CliGlobalOptionMerger.Merge(
                    [scraped],
                    [scraped with { IsCollection = true }]))
                .Throws<InvalidOperationException>();
            await Assert.That(() => CliGlobalOptionMerger.Merge(
                    [scraped],
                    [scraped with { ValueArity = CliOptionValueArity.Optional }]))
                .Throws<InvalidOperationException>();
            await Assert.That(() => CliGlobalOptionMerger.Merge(
                    [scraped],
                    [scraped with { Phase = CommandLinePhase.Terminal }]))
                .Throws<InvalidOperationException>();
        }
    }

    [Test]
    public async Task Merge_Rejects_Different_Switches_With_The_Same_Property()
    {
        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [Option("--output", "Output")],
                [Option("--destination", "Output")]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Output");
    }

    [Test]
    public async Task Merge_Rejects_An_Alias_Shared_By_Different_Options()
    {
        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [Option("--output", "Output") with { ShortForm = "-o" }],
                [Option("-o", "Other")]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("-o");
    }

    [Test]
    public async Task Merge_Rejects_Conflicting_Negated_Switches_For_The_Same_Option()
    {
        var scraped = Option("--feature", "Feature") with
        {
            CSharpType = "bool?",
            IsFlag = true,
            NegatedSwitchName = "--no-feature",
        };

        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [scraped],
                [scraped with { NegatedSwitchName = "--disable-feature" }]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--feature");
    }

    [Test]
    public async Task Merge_Rejects_A_Negated_Alias_Shared_By_Different_Options()
    {
        await Assert.That(() => CliGlobalOptionMerger.Merge(
                [Option("--feature", "Feature") with { NegatedSwitchName = "--no-feature" }],
                [Option("--no-feature", "Other")]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--no-feature");
    }

    private static CliOptionDefinition Option(string switchName, string propertyName) => new()
    {
        SwitchName = switchName,
        PropertyName = propertyName,
        CSharpType = "string?",
        ValueSeparator = "=",
    };

    private static CliEnumDefinition EnumDefinition() => new()
    {
        EnumName = "OutputFormat",
        Description = "Output format.",
        Values =
        [
            new CliEnumValue
            {
                MemberName = "Json",
                CliValue = "json",
                Description = "JSON output.",
            },
        ],
    };
}
