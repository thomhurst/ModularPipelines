using System.Globalization;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests.Attributes;

public class CliAttributeTests
{
    [Test]
    public async Task Numeric_And_Formattable_Values_Use_Invariant_Culture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var date = new DateTime(2026, 1, 2, 3, 4, 5);

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

            var arguments = BuildArguments(new TestCliOptionsWithFormattableValues
            {
                Double = 1.5,
                Decimal = 2.75m,
                Date = date,
                Values = [3.5, 4.5],
            });

            await Assert.That(arguments).IsEquivalentTo(
            [
                "--double", "1.5",
                "--decimal", "2.75",
                "--date", date.ToString(null, CultureInfo.InvariantCulture),
                "--values", "3.5", "--values", "4.5",
            ]);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Test]
    public async Task Parser_Uses_Flag_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d" };

        await Assert.That(RenderFlag(attribute)).IsEquivalentTo(["--debug"]);
    }

    [Test]
    public async Task Parser_Uses_Flag_ShortForm_When_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d", PreferShortForm = true };

        await Assert.That(RenderFlag(attribute)).IsEquivalentTo(["-d"]);
    }

    [Test]
    public async Task Parser_Uses_Flag_Name_When_Preferred_ShortForm_Is_Null()
    {
        var attribute = new CliFlagAttribute("--debug") { PreferShortForm = true };

        await Assert.That(RenderFlag(attribute)).IsEquivalentTo(["--debug"]);
    }

    [Test]
    [Arguments(OptionFormat.SpaceSeparated, "--namespace|value")]
    [Arguments(OptionFormat.EqualsSeparated, "--namespace=value")]
    [Arguments(OptionFormat.ColonSeparated, "--namespace:value")]
    [Arguments(OptionFormat.NoSeparator, "--namespacevalue")]
    public async Task Parser_Uses_Configured_Option_Separator(OptionFormat format, string expected)
    {
        var attribute = new CliOptionAttribute("--namespace") { Format = format };

        await Assert.That(string.Join('|', RenderOption(attribute))).IsEqualTo(expected);
    }

    [Test]
    public async Task Parser_Uses_Option_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n" };

        await Assert.That(RenderOption(attribute)).IsEquivalentTo(["--namespace", "value"]);
    }

    [Test]
    public async Task Parser_Uses_Option_ShortForm_When_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n", PreferShortForm = true };

        await Assert.That(RenderOption(attribute)).IsEquivalentTo(["-n", "value"]);
    }

    [Test]
    public async Task CliArgument_Defaults_To_Passthrough_Phase()
    {
        var attribute = new CliArgumentAttribute(0);

        await Assert.That(attribute.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(attribute.Required).IsFalse();
    }

    [Test]
    public async Task CliArgument_Position_Is_Set_Correctly()
    {
        var attribute = new CliArgumentAttribute(2);

        await Assert.That(attribute.Position).IsEqualTo(2);
    }

    [Test]
    public async Task Parser_Prepends_Option_Terminator_To_Passthrough_Arguments()
    {
        var options = new TestCliOptionsWithPassthroughArguments
        {
            Args = ["first", "second"],
        };

        var list = BuildArguments(options);

        await Assert.That(string.Join(' ', list)).IsEqualTo("-- first second");
    }

    [Test]
    public async Task Parser_Handles_CliFlag()
    {
        var options = new TestCliOptionsWithFlag { Debug = true };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug" });
    }

    [Test]
    public async Task Parser_Omits_CliFlag_When_False()
    {
        var options = new TestCliOptionsWithFlag { Debug = false };
        var list = BuildArguments(options);

        await Assert.That(list).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Parser_Omits_CliFlag_When_Null()
    {
        var options = new TestCliOptionsWithFlag { Debug = null };
        var list = BuildArguments(options);

        await Assert.That(list).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Parser_Repeats_Counted_CliFlag()
    {
        var options = new TestCliOptionsWithCountedFlag { Verbose = 3 };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--verbose", "--verbose", "--verbose" });
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Space_Separator()
    {
        var options = new TestCliOptionsWithOption { Namespace = "default" };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--namespace", "default" });
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Equals_Separator()
    {
        var options = new TestCliOptionsWithEqualsSeparator { Set = "key=value" };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--set=key=value" });
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Multiple_Values()
    {
        var options = new TestCliOptionsWithMultipleValues { Values = ["file1.yaml", "file2.yaml"] };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--values", "file1.yaml", "--values", "file2.yaml" });
    }

    [Test]
    public async Task Parser_Groups_CliOption_Collection_Values()
    {
        var options = new TestCliOptionsWithGroupedValues { Values = ["first", "second"] };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--values", "first", "second" });
    }

    [Test]
    public async Task Parser_Groups_CliOption_Value_Pairs()
    {
        var options = new TestCliOptionsWithGroupedPairs
        {
            Values = [new("first", "one"), new("second", "two")],
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            new[] { "--values", "first", "one", "second", "two" });
    }

    [Test]
    public async Task Parser_Rejects_Grouped_Value_Pairs_With_NonSpace_Separator()
    {
        var options = new TestCliOptionsWithInvalidGroupedPairs
        {
            Values = [new("first", "one")],
        };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("must use a space separator");
    }

    [Test]
    public async Task Parser_Handles_Space_Separated_Value_Pairs()
    {
        var options = new TestCliOptionsWithValuePairs
        {
            Values = [new CliValuePair("name", "Ada"), new CliValuePair("environment", "ci")],
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--arg", "name", "Ada", "--arg", "environment", "ci"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Rejects_NonSpace_Separated_Value_Pairs()
    {
        var options = new TestCliOptionsWithInvalidValuePairFormat
        {
            Values = [new CliValuePair("name", "Ada")],
        };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("OptionFormat.SpaceSeparated");
    }

    [Test]
    public async Task Parser_Rejects_Grouped_Values_With_NonSpace_Separator()
    {
        var options = new TestCliOptionsWithInvalidGroupedValues { Values = ["first", "second"] };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("must use a space separator");
    }

    [Test]
    public async Task Parser_Renders_Bare_OptionalValue_Option()
    {
        var options = new TestCliOptionsWithSemanticPhases
        {
            Normal = true,
            Terminal = string.Empty,
            Passthrough = "input.txt",
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--normal", "input.txt", "--terminal"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Renders_OptionalValue_And_Orders_By_Semantic_Phase()
    {
        var options = new TestCliOptionsWithSemanticPhases
        {
            EarlyOperand = "command-input",
            Normal = true,
            Terminal = "tests.txt",
            TerminalOperand = "terminal-input",
            Passthrough = "input.txt",
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["command-input", "--normal", "input.txt", "terminal-input", "--terminal", "tests.txt"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Renders_EndOfOptions_Before_Passthrough()
    {
        var list = BuildArguments(new TestCliOptionsWithSemanticPhases
        {
            Normal = true,
            EndOfOptions = true,
            Passthrough = "-input.txt",
        });

        await Assert.That(list).IsEquivalentTo(
            ["--normal", "--", "-input.txt"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Rejects_Terminal_Option_After_EndOfOptions()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithSemanticPhases
        {
            Terminal = "tests.txt",
            EndOfOptions = true,
        })).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task CommandModel_Rejects_Duplicate_Switches()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithDuplicateSwitch()))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Parser_Handles_CliArgument_After_Options()
    {
        var options = new TestCliOptionsWithArgumentAfterOptions
        {
            ReleaseName = "myrelease",
            Debug = true,
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug", "myrelease" });
    }

    [Test]
    public async Task Parser_Handles_CliArgument_Before_Options()
    {
        var options = new TestCliOptionsWithArgumentBeforeOptions
        {
            Path = "/some/path",
            Debug = true,
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "/some/path", "--debug" });
    }

    [Test]
    public async Task Parser_Omits_Null_CliArgument()
    {
        var options = new TestCliOptionsWithOptionalArgument { ReleaseName = null, Debug = true };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug" });
    }

    [Test]
    public async Task Parser_Rejects_Null_Required_CliArgument()
    {
        var options = new TestCliOptionsWithRequiredArgument { Chart = null };

        await Assert.That(() => BuildArguments(options))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("TestCliOptionsWithRequiredArgument.Chart");
    }

    [Test]
    public async Task Parser_Rejects_Blank_Required_CliArgument()
    {
        var options = new TestCliOptionsWithRequiredArgument { Chart = "   " };

        await Assert.That(() => BuildArguments(options))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("cannot be null or empty");
    }

    [Test]
    public async Task Parser_Rejects_Empty_Required_CliArgument_Collection()
    {
        var options = new TestCliOptionsWithRequiredArgumentCollection { Files = [] };

        await Assert.That(() => BuildArguments(options))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("TestCliOptionsWithRequiredArgumentCollection.Files");
    }

    [Test]
    public async Task Required_Argument_Is_Materialized_Once()
    {
        var values = new SinglePassEnumerable(["chart"]);
        var options = new TestCliOptionsWithRequiredSinglePassArgument(values);

        var arguments = BuildArguments(options);

        await Assert.That(arguments).IsEquivalentTo(["chart"]);
        await Assert.That(options.GetterCount).IsEqualTo(1);
    }

    [Test]
    public async Task Parser_Orders_Multiple_Arguments_By_Position()
    {
        var options = new TestCliOptionsWithMultipleArguments
        {
            ReleaseName = "myrelease",
            ChartReference = "bitnami/nginx",
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "myrelease", "bitnami/nginx" });
    }

    [Test]
    public async Task Parser_Handles_Mixed_Flags_Options_And_Arguments()
    {
        var options = new TestCliOptionsComplete
        {
            ReleaseName = "myrelease",
            ChartReference = "bitnami/nginx",
            Namespace = "production",
            Debug = true,
            Set = ["key1=val1", "key2=val2"],
        };
        var list = BuildArguments(options);

        await Assert.That(list[0]).IsEqualTo("--debug");
        await Assert.That(list).Contains("--namespace");
        await Assert.That(list).Contains("production");
        await Assert.That(list).Contains("--set=key1=val1");
        await Assert.That(list).Contains("--set=key2=val2");
        await Assert.That(list).Contains("myrelease");
        await Assert.That(list).Contains("bitnami/nginx");
    }

    private static IReadOnlyList<string> RenderFlag(CliFlagAttribute attribute) =>
        new CommandArgumentBuilder().BuildArguments(
            [new FlagPart("Value", _ => true, attribute)],
            new object());

    private static IReadOnlyList<string> RenderOption(CliOptionAttribute attribute) =>
        new CommandArgumentBuilder().BuildArguments(
            [new OptionPart("Value", _ => "value", attribute)],
            new object());

    // Test option classes
    internal record TestCliOptionsWithFormattableValues : CommandLineToolOptions
    {
        [CliOption("--double")]
        public double Double { get; init; }

        [CliOption("--decimal")]
        public decimal Decimal { get; init; }

        [CliOption("--date")]
        public DateTime Date { get; init; }

        [CliOption("--values")]
        public double[]? Values { get; init; }
    }

    internal record TestCliOptionsWithFlag : CommandLineToolOptions
    {
        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    internal record TestCliOptionsWithCountedFlag : CommandLineToolOptions
    {
        [CliFlag("--verbose")]
        public int? Verbose { get; set; }
    }

    internal record TestCliOptionsWithOption : CommandLineToolOptions
    {
        [CliOption("--namespace")]
        public string? Namespace { get; set; }
    }

    internal record TestCliOptionsWithEqualsSeparator : CommandLineToolOptions
    {
        [CliOption("--set", Format = OptionFormat.EqualsSeparated)]
        public string? Set { get; set; }
    }

    internal record TestCliOptionsWithMultipleValues : CommandLineToolOptions
    {
        [CliOption("--values")]
        public string[]? Values { get; set; }
    }

    internal record TestCliOptionsWithGroupedValues : CommandLineToolOptions
    {
        [CliOption("--values", GroupValues = true)]
        public string[]? Values { get; set; }
    }

    internal record TestCliOptionsWithInvalidGroupedValues : CommandLineToolOptions
    {
        [CliOption("--values", Format = OptionFormat.EqualsSeparated, GroupValues = true)]
        public string[]? Values { get; set; }
    }

    internal record TestCliOptionsWithGroupedPairs : CommandLineToolOptions
    {
        [CliOption("--values", GroupValues = true)]
        public IEnumerable<CliValuePair>? Values { get; set; }
    }

    internal record TestCliOptionsWithInvalidGroupedPairs : CommandLineToolOptions
    {
        [CliOption("--values", Format = OptionFormat.EqualsSeparated, GroupValues = true)]
        public IEnumerable<CliValuePair>? Values { get; set; }
    }

    internal record TestCliOptionsWithValuePairs : CommandLineToolOptions
    {
        [CliOption("--arg")]
        public IReadOnlyList<CliValuePair>? Values { get; set; }
    }

    internal record TestCliOptionsWithInvalidValuePairFormat : CommandLineToolOptions
    {
        [CliOption("--arg", Format = OptionFormat.EqualsSeparated)]
        public IReadOnlyList<CliValuePair>? Values { get; set; }
    }

    internal record TestCliOptionsWithSemanticPhases : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? EarlyOperand { get; set; }

        [CliFlag("--", Phase = CommandLinePhase.EndOfOptions)]
        public bool? EndOfOptions { get; set; }

        [CliOption(
            "--terminal",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal)]
        public string? Terminal { get; set; }

        [CliArgument(0, Phase = CommandLinePhase.Terminal)]
        public string? TerminalOperand { get; set; }

        [CliArgument(0)]
        public string? Passthrough { get; set; }

        [CliFlag("--normal")]
        public bool? Normal { get; set; }
    }

    internal record TestCliOptionsWithDuplicateSwitch : CommandLineToolOptions
    {
        [CliFlag("--duplicate")]
        public bool? First { get; set; }

        [CliOption("--duplicate")]
        public string? Second { get; set; }
    }

    internal record TestCliOptionsWithArgumentAfterOptions : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    internal record TestCliOptionsWithArgumentBeforeOptions : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? Path { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    internal record TestCliOptionsWithOptionalArgument : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    internal record TestCliOptionsWithRequiredArgument : CommandLineToolOptions
    {
        [CliArgument(0, Required = true)]
        public string? Chart { get; set; }
    }

    internal record TestCliOptionsWithRequiredArgumentCollection : CommandLineToolOptions
    {
        [CliArgument(0, Required = true)]
        public IEnumerable<string>? Files { get; set; }
    }

    internal sealed record TestCliOptionsWithRequiredSinglePassArgument(IEnumerable<string> values)
        : CommandLineToolOptions
    {
        public int GetterCount { get; private set; }

        [CliArgument(0, Required = true)]
        public IEnumerable<string> Values
        {
            get
            {
                GetterCount++;
                return values;
            }
        }
    }

    private sealed class SinglePassEnumerable(IEnumerable<string> values) : IEnumerable<string>
    {
        private bool _enumerated;

        public IEnumerator<string> GetEnumerator()
        {
            if (_enumerated)
            {
                throw new InvalidOperationException("The sequence can only be enumerated once.");
            }

            _enumerated = true;
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal record TestCliOptionsWithMultipleArguments : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }
    }

    internal record TestCliOptionsWithPassthroughArguments : CommandLineToolOptions
    {
        [CliArgument(0, PrependOptionTerminator = true)]
        public IEnumerable<string>? Args { get; set; }
    }

    internal record TestCliOptionsComplete : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }

        [CliOption("--namespace")]
        public string? Namespace { get; set; }

        [CliOption("--set", Format = OptionFormat.EqualsSeparated)]
        public string[]? Set { get; set; }
    }
}
