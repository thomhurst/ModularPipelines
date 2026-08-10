using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
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
    public async Task CommandLinePhase_Preserves_Published_Ordinals()
    {
        var ordinals = Enum.GetValues<CommandLinePhase>()
            .ToDictionary(phase => phase, phase => (int) phase);

        await Assert.That(ordinals[CommandLinePhase.EarlyOperand]).IsEqualTo(0);
        await Assert.That(ordinals[CommandLinePhase.Normal]).IsEqualTo(1);
        await Assert.That(ordinals[(CommandLinePhase) 2]).IsEqualTo(2);
        await Assert.That(ordinals[CommandLinePhase.Passthrough]).IsEqualTo(3);
        await Assert.That(ordinals[CommandLinePhase.Terminal]).IsEqualTo(4);
        await Assert.That(Enum.GetName((CommandLinePhase) 2)).IsEqualTo("EndOfOptions");
        await Assert.That(typeof(CommandLinePhase)
                .GetField("EndOfOptions")!
                .GetCustomAttribute<ObsoleteAttribute>())
            .IsNotNull();
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
        var options = new TestCliOptionsWithInvalidGroupedPairs();

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithInvalidGroupedPairs).FullName}.Values");
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
        var options = new TestCliOptionsWithInvalidValuePairFormat();

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithInvalidValuePairFormat).FullName}.Values");
    }

    [Test]
    public async Task Public_Builder_Rejects_HandBuilt_Grouped_NonSpace_Option()
    {
        PropertyCommandLinePart[] model =
        [
            new OptionPart(
                "Values",
                static _ => new[] { "first", "second" },
                new CliOptionAttribute("--values")
                {
                    Format = OptionFormat.EqualsSeparated,
                    GroupValues = true,
                }),
        ];

        await Assert.That(() => new CommandArgumentBuilder().BuildArguments(model, new object()))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("OptionFormat.SpaceSeparated");
    }

    [Test]
    public async Task Registered_Metadata_Rejects_Value_Pair_NonSpace_Option()
    {
        var optionsType = typeof(RegisteredValuePairOptions<string>);
        GeneratedCommandMetadata.Register(
            optionsType,
            [
                new OptionPart(
                    nameof(RegisteredValuePairOptions<string>.Values),
                    static options => ((RegisteredValuePairOptions<string>) options).Values,
                    new CliOptionAttribute("--arg") { Format = OptionFormat.EqualsSeparated }),
            ]);
        var model = new CommandModelProvider().GetCommandModel(optionsType);
        var options = new RegisteredValuePairOptions<string>
        {
            Values = [new CliValuePair("name", "value")],
        };

        await Assert.That(() => new CommandArgumentBuilder().BuildArguments(model, options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("OptionFormat.SpaceSeparated");
    }

    [Test]
    public async Task Parser_Omits_Empty_Required_Option_Collections()
    {
        var options = new TestCliOptionsWithMultipleValues { Values = [] };

        await Assert.That(BuildArguments(options)).IsEmpty();
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task Parser_Renders_Literal_Required_Option_Values(string value)
    {
        var options = new TestCliOptionsWithOption { Namespace = value };

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--namespace", value]);
    }

    [Test]
    public async Task Parser_Renders_Literal_Grouped_Option_Values()
    {
        var options = new TestCliOptionsWithGroupedValues { Values = ["", " "] };

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--values", "", " "]);
    }

    [Test]
    public async Task Parser_Renders_Literal_Value_Pair_Operands()
    {
        var options = new TestCliOptionsWithValuePairs
        {
            Values = [new CliValuePair("", " "), new CliValuePair(" ", "")],
        };

        await Assert.That(BuildArguments(options)).IsEquivalentTo(
            ["--arg", "", " ", "--arg", " ", ""],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Rejects_Grouped_Values_With_NonSpace_Separator()
    {
        var options = new TestCliOptionsWithInvalidGroupedValues();

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithInvalidGroupedValues).FullName}.Values");
    }

    [Test]
    public async Task Parser_Omits_Empty_Grouped_Required_Option_Collections()
    {
        var options = new TestCliOptionsWithGroupedValues { Values = [] };

        await Assert.That(BuildArguments(options)).IsEmpty();
    }

    [Test]
    public async Task Parser_Omits_Empty_Required_Value_Pair_Collections()
    {
        var options = new TestCliOptionsWithValuePairs { Values = [] };

        await Assert.That(BuildArguments(options)).IsEmpty();
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
    public async Task Parser_Renders_Literal_Optional_Values()
    {
        var options = new TestCliOptionsWithMultipleOptionalValues
        {
            Output = ["", " "],
        };

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--output=", "--output= "]);
    }

    [Test]
    public async Task Parser_Groups_Optional_Values_Into_One_Occurrence()
    {
        var options = new TestCliOptionsWithGroupedOptionalValues
        {
            Output = ["first", "second"],
        };

        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(
            ["--output", "first", "second"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parser_Rejects_Null_Value_Pair_Operands()
    {
        var repeated = new TestCliOptionsWithValuePairs
        {
            Values = [new CliValuePair(null, "value")],
        };
        var grouped = new TestCliOptionsWithGroupedPairs
        {
            Values = [new CliValuePair("name", null)],
        };

        await Assert.That(() => BuildArguments(repeated))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("cannot contain null values");
        await Assert.That(() => BuildArguments(grouped))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("cannot contain null values");
    }

    [Test]
    public async Task Parser_Groups_Literal_Optional_Values()
    {
        var options = new TestCliOptionsWithGroupedOptionalValues
        {
            Output = ["", " "],
        };

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--output", "", " "]);
    }

    [Test]
    public async Task Parser_Skips_Null_Repeatable_Optional_Values()
    {
        var options = new TestCliOptionsWithMultipleOptionalValues
        {
            Output = [CliOptionValue.Bare, null!, "json"],
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
    public async Task Parser_Skips_Null_Legacy_Repeatable_Optional_Values()
    {
        var options = new TestCliOptionsWithLegacyMultipleOptionalValues
        {
            Output = [string.Empty, null!, "json"],
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
        var options = new TestCliOptionsWithHandwrittenLegacyOptionalValue();

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithHandwrittenLegacyOptionalValue).FullName}.Output");
    }

    [Test]
    public async Task Parser_Rejects_Unrelated_Generated_Legacy_Optional_String_Value()
    {
        var options = new TestCliOptionsWithUnrelatedGeneratedLegacyOptionalValue();

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithUnrelatedGeneratedLegacyOptionalValue).FullName}.Output");
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
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("tests.txt")]
    public async Task CliOptionValue_Implicitly_Converts_NonNull_Strings(string value)
    {
        CliOptionValue optionValue = value;

        await Assert.That(optionValue.IsBare).IsFalse();
        await Assert.That(optionValue.Value).IsEqualTo(value);
    }

    [Test]
    public async Task Parser_Rejects_Early_Operand_Terminator_Before_Normal_Flag()
    {
        var options = new TestCliOptionsWithEarlyTerminator
        {
            Operand = "-operand",
            Normal = true,
        };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("before a later flag or option");
    }

    [Test]
    public async Task Parser_Rejects_Conditional_Early_Terminator_Before_Normal_Flag()
    {
        var options = new TestCliOptionsWithConditionalEarlyTerminator
        {
            Operand = "-operand",
            Normal = true,
        };

        await Assert.That(() => BuildArguments(options))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("before a later flag or option");
    }

    [Test]
    public async Task Parser_Rejects_Terminal_Option_After_Legacy_End_Marker()
    {
        PropertyCommandLinePart[] model =
        [
            new FlagPart(
                "EndOfOptions",
                static _ => true,
                new CliFlagAttribute("--") { Phase = (CommandLinePhase) 2 }),
            new OptionPart(
                "RunTests",
                static _ => "tests.jq",
                new CliOptionAttribute("--run-tests") { Phase = CommandLinePhase.Terminal }),
        ];

        await Assert.That(() => new CommandArgumentBuilder().BuildArguments(model, new object()))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("legacy end-of-options marker");
    }

    [Test]
    public async Task CommandModel_Rejects_Duplicate_Switches()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithDuplicateSwitch()))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task CommandModel_Rejects_Unsupported_Flag_Type_When_Unset()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithInvalidFlagType()))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                $"{typeof(TestCliOptionsWithInvalidFlagType).FullName}.Force");
    }

    [Test]
    public async Task Reflection_CommandModel_Rejects_Unsupported_Flag_Type()
    {
        var optionsType = typeof(ReflectionInvalidFlagOptions<string>);

        await Assert.That(() => new CommandModelProvider().GetCommandModel(optionsType))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining($"{optionsType.FullName}.Force");
    }

    [Test]
    public async Task Reflection_CommandModel_Rejects_Inherited_Duplicate_Argument_Position()
    {
        var optionsType = typeof(ReflectionDuplicateOverrideArgumentOptions<string>);

        await Assert.That(() => new CommandModelProvider().GetCommandModel(optionsType))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("First")
            .And.HasMessageContaining("Second");
    }

    [Test]
    public async Task CommandModel_Rejects_Duplicate_Argument_Positions_In_Phase()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithDuplicateArgumentPosition()))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("First")
            .And.HasMessageContaining("Second");
    }

    [Test]
    public async Task CommandModel_Preserves_Legacy_Default_Argument_Positions()
    {
        var arguments = BuildArguments(new TestCliOptionsWithLegacyDefaultArgumentPositions
        {
            First = "first",
            Second = "second",
        });

        await Assert.That(string.Join('|', arguments)).IsEqualTo("first|second");
    }

    [Test]
    public async Task CommandModel_Rejects_Mixed_Implicit_And_Explicit_Argument_Positions()
    {
        await Assert.That(() => BuildArguments(new TestCliOptionsWithMixedArgumentPositions()))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("First")
            .And.HasMessageContaining("Second");
    }

    [Test]
    public async Task CommandModel_Preserves_Shipped_Multi_Operand_Options()
    {
        var arguments = BuildArguments(
            new ModularPipelines.Options.Linux.AptGet.AptGetInstallOptions("curl"));

        await Assert.That(string.Join('|', arguments)).IsEqualTo("--assume-yes|install|curl");
    }

    [Test]
    public async Task CommandModel_Allows_Argument_Positions_In_Separate_Scopes()
    {
        await Assert.That(() =>
                new CommandModelProvider().GetCommandModel(typeof(TestCliOptionsWithScopedArgumentPositions)))
            .ThrowsNothing();
    }

    [Test]
    public async Task CommandModel_Rejects_Terminal_Argument_Positions_Across_Scopes()
    {
        await Assert.That(() =>
                new CommandModelProvider().GetCommandModel(typeof(TestCliOptionsWithScopedTerminalPositions)))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Global")
            .And.HasMessageContaining("Command");
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
            .And.HasMessageContaining("TestCliOptionsWithRequiredArgument.Chart")
            .And.HasMessageContaining("cannot be null or empty");
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task Parser_Renders_Literal_Required_CliArgument(string value)
    {
        var options = new TestCliOptionsWithRequiredArgument { Chart = value };

        await Assert.That(BuildArguments(options)).IsEquivalentTo([value]);
    }

    [Test]
    public async Task Parser_Rejects_Empty_Required_CliArgument_Collection()
    {
        var options = new TestCliOptionsWithRequiredArgumentCollection { Files = [] };

        await Assert.That(() => BuildArguments(options))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("TestCliOptionsWithRequiredArgumentCollection.Files")
            .And.HasMessageContaining("cannot be null or empty");
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
        [CliOption(
            "--terminal",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal)]
        public CliOptionValue? Terminal { get; set; }

        [CliArgument(0, Phase = CommandLinePhase.Terminal)]
        public string? TerminalOperand { get; set; }

        [CliArgument(0)]
        public string? Passthrough { get; set; }

        [CliFlag("--normal")]
        public bool? Normal { get; set; }
    }

    private record TestCliOptionsWithMultipleOptionalValues
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
            ValueArity = CliOptionValueArity.Optional)]
        public IEnumerable<CliOptionValue>? Output { get; set; }
    }

    private record TestCliOptionsWithGroupedOptionalValues
    {
        [CliOption(
            "--output",
            ValueArity = CliOptionValueArity.Optional,
            GroupValues = true)]
        public IEnumerable<CliOptionValue>? Output { get; set; }
    }

    [GeneratedCode("ModularPipelines.OptionsGenerator", "3.0.0")]
    private record TestCliOptionsWithLegacyMultipleOptionalValues
    {
        [CliOption(
            "--output",
            Format = OptionFormat.EqualsSeparated,
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

    internal record TestCliOptionsWithDuplicateSwitch : CommandLineToolOptions
    {
        [CliFlag("--duplicate")]
        public bool? First { get; set; }

        [CliOption("--duplicate")]
        public string? Second { get; set; }
    }

    internal record TestCliOptionsWithInvalidFlagType : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public string? Force { get; set; }
    }

    private sealed record ReflectionInvalidFlagOptions<T> : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public T? Force { get; init; }
    }

    private record ReflectionExplicitArgumentBase<T> : CommandLineToolOptions
    {
        [CliArgument(0)]
        public virtual string? First { get; init; }
    }

    private sealed record ReflectionDuplicateOverrideArgumentOptions<T>
        : ReflectionExplicitArgumentBase<T>
    {
        public override string? First { get; init; }

        [CliArgument(0)]
        public string? Second { get; init; }
    }

    private sealed record RegisteredValuePairOptions<T> : CommandLineToolOptions
    {
        public IReadOnlyList<CliValuePair>? Values { get; init; }
    }

    internal record TestCliOptionsWithDuplicateArgumentPosition : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? First { get; set; }

        [CliArgument(0)]
        public string? Second { get; set; }
    }

    internal record TestCliOptionsWithLegacyDefaultArgumentPositions : CommandLineToolOptions
    {
        [CliArgument]
        public string? First { get; set; }

        [CliArgument]
        public string? Second { get; set; }
    }

    internal record TestCliOptionsWithMixedArgumentPositions : CommandLineToolOptions
    {
        [CliArgument]
        public string? First { get; set; }

        [CliArgument(0)]
        public string? Second { get; set; }
    }

    [CliGlobalOptions]
    internal record TestCliGlobalArgumentOptions : CommandLineToolOptions
    {
        [CliArgument(0)]
        public string? Global { get; set; }
    }

    internal record TestCliOptionsWithScopedArgumentPositions : TestCliGlobalArgumentOptions
    {
        [CliArgument(0)]
        public string? Command { get; set; }
    }

    [CliGlobalOptions]
    internal record TestCliGlobalTerminalArgumentOptions : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.Terminal)]
        public string? Global { get; set; }
    }

    internal record TestCliOptionsWithScopedTerminalPositions : TestCliGlobalTerminalArgumentOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.Terminal)]
        public string? Command { get; set; }
    }

    internal record TestCliOptionsWithEarlyTerminator : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand, PrependOptionTerminator = true)]
        public string? Operand { get; set; }

        [CliFlag("--normal")]
        public bool? Normal { get; set; }
    }

    internal record TestCliOptionsWithConditionalEarlyTerminator : CommandLineToolOptions
    {
        [CliArgument(
            0,
            Phase = CommandLinePhase.EarlyOperand,
            PrependOptionTerminatorIfValueStartsWithDash = true)]
        public string? Operand { get; set; }

        [CliFlag("--normal")]
        public bool? Normal { get; set; }
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
