using System.CodeDom.Compiler;
using System.Globalization;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests.Attributes;

public class CliAttributeTests
{
    [Test]
    public async Task Named_Placeholder_Values_Use_Invariant_Culture()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
            var handler = new PlaceholderHandler(new CommandModelProvider());

            var arguments = handler.ReplacePlaceholders(
                ["<SCALAR>", "<VALUES>"],
                new TestCliOptionsWithNamedFormattableValues
                {
                    Scalar = 1.5,
                    Values = [2.5, 3.5],
                });

            await Assert.That(arguments).IsEquivalentTo(
                ["1.5", "2.5", "3.5"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

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
    public async Task CliFlag_Returns_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d" };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--debug");
    }

    [Test]
    public async Task CliFlag_Returns_ShortForm_When_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d", PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("-d");
    }

    [Test]
    public async Task CliFlag_Returns_Name_When_ShortForm_Null_And_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--debug");
    }

    [Test]
    [Arguments(OptionFormat.SpaceSeparated, " ")]
    [Arguments(OptionFormat.EqualsSeparated, "=")]
    [Arguments(OptionFormat.ColonSeparated, ":")]
    [Arguments(OptionFormat.NoSeparator, "")]
    public async Task CliOption_GetSeparator_Returns_Correct_Separator(OptionFormat format, string expected)
    {
        var attribute = new CliOptionAttribute("--namespace") { Format = format };

        await Assert.That(attribute.GetSeparator()).IsEqualTo(expected);
    }

    [Test]
    public async Task CliOption_CustomSeparator_Overrides_Format()
    {
        var attribute = new CliOptionAttribute("--namespace")
        {
            Format = OptionFormat.SpaceSeparated,
            CustomSeparator = "::",
        };

        await Assert.That(attribute.GetSeparator()).IsEqualTo("::");
    }

    [Test]
    public async Task CliOption_Returns_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n" };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--namespace");
    }

    [Test]
    public async Task CliOption_Returns_ShortForm_When_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n", PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("-n");
    }

    [Test]
    public async Task CliArgument_Defaults_To_AfterOptions_Placement()
    {
        var attribute = new CliArgumentAttribute(0);

        await Assert.That(attribute.Placement).IsEqualTo(ArgumentPlacement.AfterOptions);
        await Assert.That(attribute.Phase).IsEqualTo(CommandLinePhase.Passthrough);
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
    public async Task Parser_Rejects_Empty_Required_Option_Collections()
    {
        var options = new TestCliOptionsWithMultipleValues { Values = [] };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining($"{typeof(TestCliOptionsWithMultipleValues).FullName}.Values");
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task Parser_Rejects_Empty_Required_Option_Values(string value)
    {
        var options = new TestCliOptionsWithOption { Namespace = value };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining($"{typeof(TestCliOptionsWithOption).FullName}.Namespace");
    }

    [Test]
    public async Task Parser_Renders_Bare_OptionalValue_Option()
    {
        var options = new TestCliOptionsWithSemanticPhases
        {
            Normal = true,
            Terminal = CliOptionValue.Bare,
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
            Normal = true,
            Terminal = "tests.txt",
            Passthrough = "input.txt",
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--normal", "input.txt", "--terminal", "tests.txt"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Renders_Multiple_Bare_And_Explicit_Optional_Values()
    {
        var options = new TestCliOptionsWithMultipleOptionalValues
        {
            Output = [CliOptionValue.Bare, "json"],
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--output", "--output=json"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Preserves_Legacy_Multiple_Optional_String_Values()
    {
        var options = new TestCliOptionsWithLegacyMultipleOptionalValues
        {
            Output = [string.Empty, "json"],
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--output", "--output=json"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Preserves_Legacy_Scalar_Optional_String_Value()
    {
        var options = new TestCliOptionsWithLegacyScalarOptionalValue { Output = "json" };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(["--output=json"]);
    }

    [Test]
    public async Task Parser_Preserves_Legacy_Optional_Value_From_Generated_Base_Type()
    {
        var options = new TestCliOptionsDerivedFromLegacyGeneratedOptions { Output = "json" };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(["--output=json"]);
    }

    [Test]
    public async Task Parser_Rejects_Handwritten_Legacy_Optional_String_Value()
    {
        var options = new TestCliOptionsWithHandwrittenLegacyOptionalValue { Output = "json" };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(nameof(CliOptionValue));
    }

    [Test]
    public async Task Parser_Rejects_Unrelated_Generated_Legacy_Optional_String_Value()
    {
        var options = new TestCliOptionsWithUnrelatedGeneratedLegacyOptionalValue { Output = "json" };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(nameof(CliOptionValue));
    }

    [Test]
    public async Task Parser_Omits_Null_OptionalValue_Option()
    {
        var list = BuildArguments(new TestCliOptionsWithSemanticPhases { Terminal = null });

        await Assert.That(list).IsEmpty();
    }

    [Test]
    public async Task Parser_Preserves_Explicit_Optional_Value_Across_Cultures()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            var list = BuildArguments(new TestCliOptionsWithSemanticPhases { Terminal = "1.5" });

            await Assert.That(list).IsEquivalentTo(["--terminal", "1.5"]);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Test]
    public async Task CliOptionValue_Implicitly_Preserves_Null_Strings()
    {
        string? value = null;
        CliOptionValue? optionValue = value;

        await Assert.That(optionValue).IsNull();
    }

    [Test]
    public async Task CliOptionValue_Implicitly_Converts_NonEmpty_Strings()
    {
        CliOptionValue optionValue = "tests.txt";

        await Assert.That(optionValue.IsBare).IsFalse();
        await Assert.That(optionValue.Value).IsEqualTo("tests.txt");
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
    public async Task Parser_Renders_None_Arity_Option_Without_Value()
    {
        var list = BuildArguments(new TestCliOptionsWithSemanticPhases
        {
            Valueless = true,
        });

        await Assert.That(list).IsEquivalentTo(["--valueless"]);
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

    // Test option classes
    private record TestCliOptionsWithFormattableValues
    {
        [CliOption("--double")]
        public double Double { get; init; }

        [CliOption("--decimal")]
        public decimal Decimal { get; init; }

        [CliOption("--date")]
        public DateTime Date { get; init; }

        [CliOption("--values", AllowMultiple = true)]
        public double[]? Values { get; init; }
    }

    private record TestCliOptionsWithNamedFormattableValues
    {
        [CliArgument(Name = "<SCALAR>")]
        public double? Scalar { get; init; }

        [CliArgument(Name = "<VALUES>")]
        public IEnumerable<double>? Values { get; init; }
    }

    private record TestCliOptionsWithFlag
    {
        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithCountedFlag
    {
        [CliFlag("--verbose")]
        public int? Verbose { get; set; }
    }

    private record TestCliOptionsWithOption
    {
        [CliOption("--namespace")]
        public string? Namespace { get; set; }
    }

    private record TestCliOptionsWithEqualsSeparator
    {
        [CliOption("--set", Format = OptionFormat.EqualsSeparated)]
        public string? Set { get; set; }
    }

    private record TestCliOptionsWithMultipleValues
    {
        [CliOption("--values", AllowMultiple = true)]
        public string[]? Values { get; set; }
    }

    private record TestCliOptionsWithValuePairs
    {
        [CliOption("--arg", AllowMultiple = true)]
        public IReadOnlyList<CliValuePair>? Values { get; set; }
    }

    private record TestCliOptionsWithInvalidValuePairFormat
    {
        [CliOption("--arg", Format = OptionFormat.EqualsSeparated, AllowMultiple = true)]
        public IReadOnlyList<CliValuePair>? Values { get; set; }
    }

    private record TestCliOptionsWithSemanticPhases
    {
        [CliFlag("--", Phase = CommandLinePhase.EndOfOptions)]
        public bool? EndOfOptions { get; set; }

        [CliOption(
            "--terminal",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal)]
        public CliOptionValue? Terminal { get; set; }

        [CliArgument(0)]
        public string? Passthrough { get; set; }

        [CliFlag("--normal")]
        public bool? Normal { get; set; }

        [CliOption("--valueless", ValueArity = CliOptionValueArity.None)]
        public bool? Valueless { get; set; }
    }

    private record TestCliOptionsWithMultipleOptionalValues
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            AllowMultiple = true,
            ValueArity = CliOptionValueArity.Optional)]
        public IEnumerable<CliOptionValue>? Output { get; set; }
    }

    [GeneratedCode("ModularPipelines.OptionsGenerator", "3.0.0")]
    private record TestCliOptionsWithLegacyMultipleOptionalValues
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            AllowMultiple = true,
            ValueArity = CliOptionValueArity.Optional)]
        public IEnumerable<string>? Output { get; set; }
    }

    [GeneratedCode("ModularPipelines.OptionsGenerator", "3.0.0")]
    private record TestCliOptionsWithLegacyScalarOptionalValue
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            ValueArity = CliOptionValueArity.Optional)]
        public string? Output { get; set; }
    }

    [GeneratedCode("ModularPipelines.OptionsGenerator", "3.0.0")]
    private record TestCliOptionsWithLegacyGeneratedBase
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            ValueArity = CliOptionValueArity.Optional)]
        public string? Output { get; set; }
    }

    private sealed record TestCliOptionsDerivedFromLegacyGeneratedOptions
        : TestCliOptionsWithLegacyGeneratedBase;

    private record TestCliOptionsWithHandwrittenLegacyOptionalValue
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            ValueArity = CliOptionValueArity.Optional)]
        public string? Output { get; set; }
    }

    [GeneratedCode("Other.Generator", "1.0.0")]
    private record TestCliOptionsWithUnrelatedGeneratedLegacyOptionalValue
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            ValueArity = CliOptionValueArity.Optional)]
        public string? Output { get; set; }
    }

    private record TestCliOptionsWithDuplicateSwitch
    {
        [CliFlag("--duplicate")]
        public bool? First { get; set; }

        [CliOption("--duplicate")]
        public string? Second { get; set; }
    }

    private record TestCliOptionsWithArgumentAfterOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithArgumentBeforeOptions
    {
        [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
        public string? Path { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithOptionalArgument
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithMultipleArguments
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }
    }

    private record TestCliOptionsWithPassthroughArguments
    {
        [CliArgument(0, PrependOptionTerminator = true)]
        public IEnumerable<string>? Args { get; set; }
    }

    private record TestCliOptionsComplete
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }

        [CliOption("--namespace")]
        public string? Namespace { get; set; }

        [CliOption("--set", Format = OptionFormat.EqualsSeparated, AllowMultiple = true)]
        public string[]? Set { get; set; }
    }
}
