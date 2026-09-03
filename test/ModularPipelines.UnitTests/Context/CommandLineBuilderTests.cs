using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Secrets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

using ModularPipelines.Generated;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class CommandLineBuilderTests : TestBase
{
    [Test]
    public async Task Build_FromGenericOptions_ReturnsCorrectCommandLine()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new CommandLineToolOptions("echo")
        {
            Arguments = ["hello", "world"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("echo");
        await Assert.That(result.Arguments).IsEquivalentTo(new[] { "hello", "world" });
    }

    [Test]
    public async Task Build_Rejects_All_Invalid_DataAnnotation_Values()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestValidatedOptions
        {
            Verbose = 7,
            Name = "123",
        });

        var exception = await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestValidatedOptions.Name")
            .And.HasMessageContaining("TestValidatedOptions.Verbose");
        await Assert.That(exception!.InnerException).IsTypeOf<ValidationException>();
    }

    [Test]
    public async Task Build_Rejects_Invalid_CliOptionValue_Annotation()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestValidatedOptions
        {
            Level = "4",
        });

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestValidatedOptions.Level");
    }

    [Test]
    public async Task Build_Uses_DataAnnotations_Display_Name_For_Public_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        await Assert.That(() => builder.Build(new TestValidatedOptions
        {
            Verbose = 7,
            Name = "valid",
        }))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("Verbosity level");
    }

    [Test]
    public async Task Build_Skips_Other_Property_Validation_When_Required_Fails()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestRequiredShortCircuitOptions());

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestRequiredShortCircuitOptions.Value");
    }

    [Test]
    public async Task Build_Evaluates_Remaining_Required_Validators()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestMultipleRequiredOptions());

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("Second required validator failed.");
    }

    [Test]
    public async Task Build_Rejects_Invalid_NonPublic_Annotated_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestNonPublicValidatedOptions
        {
            Retries = 4,
        });

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestNonPublicValidatedOptions.Retries");
    }

    [Test]
    public async Task Build_Rejects_Invalid_Inherited_NonPublic_Annotated_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestDerivedNonPublicValidatedOptions
        {
            Retries = 4,
        });

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestDerivedNonPublicValidatedOptions.Retries");
    }

    [Test]
    public async Task Build_Rejects_Invalid_Overridden_NonPublic_Annotated_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestOverriddenNonPublicValidatedOptions
        {
            Retries = 4,
        });

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestOverriddenNonPublicValidatedOptions.Retries")
            .And.HasMessageContaining("Retry count");
    }

    [Test]
    public async Task Build_Honors_NonInherited_Validation_Attribute_On_Overridden_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestNonInheritedValidationOverrideOptions());

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_Uses_Derived_SingleUse_Validation_Attribute_On_Overridden_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestDerivedValidationOverrideOptions
        {
            Retries = 5,
        });

        await Assert.That(result.ToString()).IsEqualTo("tool --retries 5");
    }

    [Test]
    public async Task Build_Rejects_Invalid_ProtectedInternal_Annotated_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestProtectedInternalValidatedOptions
        {
            Retries = 4,
        });

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestProtectedInternalValidatedOptions.Retries");
    }

    [Test]
    public async Task Build_Ignores_Unsupported_NonPublic_Annotated_Helper_Properties()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestUnsupportedNonPublicValidatedOptions());

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_Skips_Object_Validation_When_NonPublic_Property_Is_Invalid()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestNonPublicAndObjectValidatedOptions
        {
            Retries = 0,
        };

        CommandLine Build() => builder.Build(options);

        await Assert.That(Build)
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestNonPublicAndObjectValidatedOptions.Name")
            .And.HasMessageContaining("TestNonPublicAndObjectValidatedOptions.Retries");
        await Assert.That(options.ValidationCallbackInvoked).IsFalse();
    }

    [Test]
    public async Task Build_Skips_Type_Validation_When_Property_Is_Invalid()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestPropertyFirstValidatedOptions();

        await Assert.That(() => builder.Build(options))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestPropertyFirstValidatedOptions.Name")
            .And.HasMessageContaining("TestPropertyFirstValidatedOptions.Retries");
        await Assert.That(options.TypeValidationInvoked).IsFalse();
    }

    [Test]
    public async Task Build_Accepts_ValidationResult_Success_From_Object_Validation()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestSuccessfulObjectValidatedOptions());

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_Provides_Scoped_Services_To_Validation_Contexts()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestServiceValidatedOptions());

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_Wraps_ValidationException_From_Callback()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var exception = await Assert.That(() => builder.Build(new TestThrowingValidatedOptions()))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("Callback validation failed.");
        await Assert.That(exception!.InnerException)
            .IsTypeOf<ValidationException>()
            .And.HasMessageContaining("Callback validation failed.");
    }

    [Test]
    public async Task Build_Obfuscates_Secret_In_Validation_Failure()
    {
        const string secret = "validation-secret-token";
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestSecretValidatedOptions
        {
            Token = secret,
        };

        var exception = await Assert.That(() => builder.Build(options))
            .Throws<CommandOptionsValidationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("**********");
            await Assert.That(exception.Message).DoesNotContain(secret);
            await Assert.That(exception.InnerException!.Message).DoesNotContain(secret);
            await Assert.That(exception.ToString()).DoesNotContain(secret);
        }
    }

    [Test]
    public async Task Build_Sanitizes_NonValidationException_From_Callback()
    {
        const string secret = "throwing-validation-secret-token";
        var builder = await GetService<ICommandLineBuilder>();

        var exception = await Assert.That(() => builder.Build(new TestThrowingSecretValidatedOptions
        {
            Token = secret,
        }))
            .Throws<CommandOptionsValidationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("**********");
            await Assert.That(exception.Message).DoesNotContain(secret);
            await Assert.That(exception.InnerException).IsTypeOf<ValidationException>();
            await Assert.That(exception.ToString()).DoesNotContain(secret);
        }
    }

    [Test]
    public async Task Build_Preserves_Collected_Errors_When_Later_Validation_Throws()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var exception = await Assert.That(() => builder.Build(new TestCollectedAndThrowingOptions()))
            .Throws<CommandOptionsValidationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message)
                .Contains("TestCollectedAndThrowingOptions.Retries");
            await Assert.That(exception.Message)
                .Contains("Required validation should short-circuit this attribute.");
        }
    }

    [Test]
    public async Task Build_Accepts_Valid_DataAnnotation_Values()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestValidatedOptions
        {
            Verbose = 2,
            Name = "valid",
            Level = CliOptionValue.Bare,
        });

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_Uses_TypeDescriptor_Validation_Added_After_First_Build()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestTypeDescriptorValidatedOptions();
        var provider = new AssociatedMetadataTypeTypeDescriptionProvider(
            typeof(TestTypeDescriptorValidatedOptions),
            typeof(TestTypeDescriptorValidationMetadata));

        builder.Build(options);
        TypeDescriptor.AddProvider(provider, typeof(TestTypeDescriptorValidatedOptions));
        try
        {
            await Assert.That(() => builder.Build(options))
                .Throws<CommandOptionsValidationException>()
                .And.HasMessageContaining("TestTypeDescriptorValidatedOptions.Name");
        }
        finally
        {
            TypeDescriptor.RemoveProvider(provider, typeof(TestTypeDescriptorValidatedOptions));
        }
    }

    [Test]
    public async Task Build_Validates_Provider_Exposed_NonPublic_Property_Once()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestProviderExposedNonPublicOptions();

        await Assert.That(() => builder.Build(options))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestProviderExposedNonPublicOptions.Retries");
        await Assert.That(options.ValidationCount).IsEqualTo(1);
    }

    [Test]
    public async Task Build_Validates_Provider_Added_Attribute_On_NonPublic_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestProviderExposedNonPublicOptions
        {
            Name = null,
            RejectRetries = false,
        };

        await Assert.That(() => builder.Build(options))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestProviderExposedNonPublicOptions.Name");
        await Assert.That(options.ValidationCount).IsEqualTo(1);
    }

    [Test]
    public async Task Build_Skips_Provider_Validators_After_NonPublic_Required_Failure()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestProviderExposedNonPublicOptions
        {
            RequiredValue = null,
            RejectRetries = false,
        };

        await Assert.That(() => builder.Build(options))
            .Throws<CommandOptionsValidationException>()
            .And.HasMessageContaining("TestProviderExposedNonPublicOptions.RequiredValue");
        await Assert.That(options.ProviderValidationCount).IsEqualTo(0);
    }

    [Test]
    public async Task Build_Ignores_Validated_SetterOnly_Property()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestSetterOnlyValidatedOptions());

        await Assert.That(result.Tool).IsEqualTo("tool");
    }

    [Test]
    public async Task Build_FromGenericOptions_WithRunSettings_AddsDoubleDash()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new CommandLineToolOptions("dotnet")
        {
            Arguments = ["test"],
            RunSettings = ["--filter", "Category=Unit"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("dotnet");
        await Assert.That(result.Arguments).IsEquivalentTo(new[] { "test", "--", "--filter", "Category=Unit" });
    }

    [Test]
    public async Task Build_Places_Terminal_Options_After_Manual_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["."],
            RunTests = "tests.jq",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq . --run-tests tests.jq");
    }

    [Test]
    public async Task Build_Hoists_Recognized_Manual_Option_Before_Passthrough_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--compact"],
            ArgumentsContainToolOptions = true,
            Filter = "-1",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --compact -- -1");
    }

    [Test]
    public async Task Build_Hoists_Manual_Negated_Flag_Before_Option_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestNegatedManualFlagOptions
        {
            Arguments = ["--no-feature"],
            ArgumentsContainToolOptions = true,
            Filter = "-1",
        });

        await Assert.That(result.ToString()).IsEqualTo("tool --no-feature -- -1");
    }

    [Test]
    public async Task Build_Preserves_Manual_Arguments_After_Ordinary_Passthrough_Values()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestPositionalOptions
        {
            Arguments = ["--custom-flag"],
            ConfigPath = "config.json",
        });

        await Assert.That(result.ToString()).IsEqualTo("processor config.json --custom-flag");
    }

    [Test]
    public async Task Build_Places_Terminated_Passthrough_Before_Late_Operands()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminatedPassthroughOptions("source", "destination")
        {
            Force = true,
            ScpFlags = ["-F", "config"],
        });

        await Assert.That(result.ToString())
            .IsEqualTo("tool copy --force -- -F config source destination");
    }

    [Test]
    public async Task Build_Places_Manual_Options_Before_Terminated_Passthrough()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminatedPassthroughOptions("source", "destination")
        {
            Arguments = ["--force", "--", "-F", "config"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("tool copy --force -- -F config source destination");
    }

    [Test]
    public async Task Build_Omits_Terminator_When_Terminated_Passthrough_Is_Absent()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminatedPassthroughOptions("source", "destination"));

        await Assert.That(result.ToString()).IsEqualTo("tool copy source destination");
    }

    [Test]
    public async Task Build_Rejects_Manual_Arguments_For_Missing_Required_Operand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        await Assert.That(() => builder.Build(new TestRequiredOperandCompatibilityOptions
        {
            Arguments = ["legacy-operand"],
        }))
            .Throws<ArgumentException>()
            .And.HasMessageContaining("TestRequiredOperandCompatibilityOptions.Operand");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_RunSettings()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            RunTests = "tests.jq",
            RunSettings = ["--filter", "Category=Unit"],
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            RunTests = "tests.jq",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_Manual_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "-1"],
            ArgumentsContainOptionTerminator = true,
            RunTests = "tests.jq",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Preserves_Terminal_Like_Tokens_After_Manual_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "value", "--run-tests", "tests.jq"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq -- value --run-tests tests.jq");
    }

    [Test]
    public async Task Build_Rejects_Command_Options_After_Global_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestGlobalTerminatorCommandOptions
        {
            GlobalOperand = "-operand",
            Force = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("earlier property group");
    }

    [Test]
    public async Task Build_Rejects_Global_Property_Terminator_Before_Subcommand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestGlobalTerminatorCommandOptions
        {
            GlobalOperand = "-operand",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("subcommand");
    }

    [Test]
    public async Task Build_Rejects_Manual_Options_After_Global_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestGlobalTerminatorCommandOptions
        {
            GlobalOperand = "-operand",
            Arguments = ["--force"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Manual tool options");
    }

    [Test]
    public async Task Build_Accepts_Terminal_Option_When_Option_Value_Is_DoubleDash()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Argument = new CliValuePair("name", "--"),
            RunTests = "tests.jq",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --arg name -- --run-tests tests.jq");
    }

    [Test]
    public async Task Build_Accepts_Terminal_Argument_Own_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            TerminalArgument = "-x",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq -- -x");
    }

    [Test]
    public async Task Build_Repeats_Explicit_Option_Terminator_For_Later_Argument_Group()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestRepeatedOptionTerminatorOptions
        {
            PlatformOptions = ["--filter", "Category=Unit"],
            ExtensionOptions = ["--report-trx"],
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "dotnet test -- --filter Category=Unit -- --report-trx");
    }

    [Test]
    public async Task Build_Rejects_Manual_Terminal_Option_Before_Terminal_Argument()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Arguments = ["--run-tests", "tests.jq"],
            ArgumentsContainToolOptions = true,
            TerminalArgument = "-x",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Manual terminal options");
    }

    [Test]
    public async Task Build_Rejects_Bare_Manual_Terminal_Option_After_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--run-tests"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Manual terminal options");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Option_After_Terminal_Argument_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            TerminalArgument = "-x",
            RunTests = "tests.jq",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_RunSettings_When_Terminal_Argument_Emits_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            RunSettings = ["--filter", "Category=Unit"],
            TerminalArgument = "-x",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("RunSettings");
    }

    [Test]
    public async Task Build_Manual_Terminator_Is_Reused_For_RunSettings()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "-1"],
            ArgumentsContainOptionTerminator = true,
            RunSettings = ["extra"],
        });

        await Assert.That(result.ToString()).IsEqualTo("jq -- -1 extra");
    }

    [Test]
    public async Task Build_Manual_Terminator_Is_Reused_For_Terminal_Argument()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "input"],
            ArgumentsContainOptionTerminator = true,
            TerminalArgument = "-x",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq -- input -x");
    }

    [Test]
    public async Task Build_Does_Not_Infer_Terminator_From_Manual_Option_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--arg", "name", "--", "."],
            RunSettings = ["--foo"],
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --arg name -- . -- --foo");
    }

    [Test]
    public async Task Build_Rejects_Declared_Manual_Terminator_When_Missing()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Arguments = ["value"],
            ArgumentsContainOptionTerminator = true,
        });

        var exception = Assert.Throws<ArgumentException>(() => _ = Build());

        await Assert.That(exception.ParamName)
            .IsEqualTo("options");
    }

    [Test]
    public async Task Build_Does_Not_Treat_Manual_Option_Operand_As_Declared_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Arguments = ["--arg", "name", "--"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<ArgumentException>()
            .And.HasMessageContaining("unconsumed '--'");
    }

    [Test]
    public async Task Build_Does_Not_Match_Manual_Option_Across_Declared_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--arg", "name", "--", "tail"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --arg name -- tail");
    }

    [Test]
    public async Task Build_Hoists_Manual_Option_Operands_Before_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--arg", "name", "value", "input.json"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --arg name value -- -1 input.json");
    }

    [Test]
    public async Task Build_Hoists_EqualsSeparated_Manual_Option_Before_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --color=never -- -1");
    }

    [Test]
    public async Task Build_Hoists_Manual_Option_Before_Early_Operand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestEarlyOperandTerminatorOptions("aws")
        {
            Parameters = ["param"],
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("pulumi package info --color=never aws -- param");
    }

    [Test]
    public async Task Build_Materializes_Required_Operand_Once()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TestRequiredSinglePassOperandOptions(["input"]);

        var result = builder.Build(options);

        using (Assert.Multiple())
        {
            await Assert.That(result.ToString()).IsEqualTo("test input");
            await Assert.That(options.GetterCount).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Build_Validates_Required_Early_Operand_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestRequiredEarlyOperandOptions
        {
            Operand = "-input",
            Force = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Hoists_ColonSeparated_Manual_Option_Before_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--define:value"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --define:value -- -1");
    }

    [Test]
    public async Task Build_Hoists_NoSeparator_Manual_Option_Before_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--variablevalue"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --variablevalue -- -1");
    }

    [Test]
    public async Task Build_Preserves_Exact_Option_Like_Tokens_After_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "tail", "-Debug", "value"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq -- tail -Debug value");
    }

    [Test]
    public async Task Build_Hoists_Optional_Manual_Option_With_Explicit_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--dry-run", "client"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --dry-run client -- -1");
    }

    [Test]
    public async Task Build_Preserves_Grouped_Option_Like_Tokens_After_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "tail", "--arguments", "one", "two"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq -- tail --arguments one two");
    }

    [Test]
    public async Task Build_Preserves_Undeclared_Double_Dash_In_Grouped_Values()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--arguments", "one", "--", "two"],
            ArgumentsContainToolOptions = true,
            RunSettings = ["tail"],
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --arguments one -- two -- tail");
    }

    [Test]
    public async Task Build_Preserves_Attached_Grouped_Tokens_After_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "tail", "--arguments=one", "two"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq -- tail --arguments=one two");
    }

    [Test]
    public async Task Build_Grouped_Option_Preserves_Terminal_Option_For_Validation()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--arguments", "one", "--run-tests", "tests.jq"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Manual terminal options");
    }

    [Test]
    public async Task Build_Preserves_Terminal_Status_In_Manual_Short_Cluster()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var modelProvider = await GetService<ICommandModelProvider>();
        var flags = modelProvider.GetCommandModel(typeof(TestTerminalOptions))
            .OfType<FlagPart>()
            .ToDictionary(flag => flag.Attribute.ShortForm!);

        using (Assert.Multiple())
        {
            await Assert.That(flags["-c"].Phase).IsEqualTo(CommandLinePhase.Normal);
            await Assert.That(flags["-T"].Phase).IsEqualTo(CommandLinePhase.Terminal);
        }

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["-cT"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Manual terminal options");
    }

    [Test]
    public async Task Reflection_Metadata_Counts_Derived_Value_Pairs()
    {
        var model = new CommandModelProvider()
            .GetCommandModel(typeof(ReflectionDerivedPairOptions<DerivedCliValuePair>));

        await Assert.That(model.OfType<OptionPart>().Single().ManualOperandCount)
            .IsEqualTo(2);
    }

    [Test]
    public async Task Build_Allows_Zero_Manual_Operands_From_Runtime_Metadata()
    {
        var modelProvider = new Mock<ICommandModelProvider>();
        modelProvider
            .Setup(x => x.GetCommandModel(typeof(TestZeroManualOperandOptions)))
            .Returns(
            [
                new OptionPart(
                    "Zero",
                    static _ => null,
                    new CliOptionAttribute("--zero"))
                {
                    ManualOperandCount = 0,
                },
            ]);
        var (builder, _) = await GetService<ICommandLineBuilder>(services =>
            services.AddSingleton(modelProvider.Object));

        var result = builder.Build(new TestZeroManualOperandOptions
        {
            Arguments = ["--zero", "--", "tail"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString()).IsEqualTo("tool --zero -- tail");
    }

    [Test]
    public async Task Build_Preserves_Command_Option_Operand_That_Matches_Global_Flag()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestGlobalCommandOptions
        {
            Input = "-input",
            Arguments = ["--set", "--verbose"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("liquibase update --set --verbose -- -input");
    }

    [Test]
    public async Task Build_Preserves_Manual_Tokens_After_Manual_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestManualTerminatorOptions
        {
            Filter = ".",
            Arguments = ["--", "input.json", "--compact-output"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq . -- input.json --compact-output");
    }

    [Test]
    public async Task Build_Hoists_Manual_Global_Option_Before_Command()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestGlobalCommandOptions
        {
            Input = "-input",
            Arguments = ["--verbose"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("liquibase --verbose update -- -input");
    }

    [Test]
    public async Task Build_Rejects_Manual_Short_Cluster_With_Mixed_Scopes()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestGlobalCommandOptions
        {
            Input = "-input",
            Arguments = ["-vf"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("cannot mix global and command-specific options");
    }

    [Test]
    public async Task Build_Preserves_MultiCharacter_Option_Like_Tokens_After_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestMultiCharacterShortOptionOptions
        {
            Arguments = ["--", "tail", "-ss", "https://symbols"],
            ArgumentsContainOptionTerminator = true,
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("dotnet -- tail -ss https://symbols");
    }

    [Test]
    public async Task Build_Preserves_Manual_Option_Override_Order_When_Hoisted()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Color = "always",
            Filter = "-1",
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(result.ToString())
            .IsEqualTo("jq --color=always --color=never -- -1");
    }

    [Test]
    public async Task Build_Rejects_Manual_Terminator_After_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            Arguments = ["--", "input.json"],
            ArgumentsContainOptionTerminator = true,
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("already emitted");
    }

    [Test]
    public async Task Build_Empty_RunSettings_Emit_No_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new CommandLineToolOptions("dotnet")
        {
            Arguments = ["test"],
            RunSettings = [],
        });

        await Assert.That(result.ToString()).IsEqualTo("dotnet test");
    }

    [Test]
    public async Task Build_Rejects_RunSettings_When_Passthrough_Argument_Emits_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            RunSettings = ["extra"],
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("RunSettings");
    }

    [Test]
    public async Task Build_FromAttributeBasedOptions_ResolvesToolAndSubcommands()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new TestAttributeOptions
        {
            Force = true,
            Output = "/path/to/output"
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("mytool");
        await Assert.That(result.Arguments).Contains("sub");
        await Assert.That(result.Arguments).Contains("command");
        await Assert.That(result.Arguments).Contains("--force");
        await Assert.That(result.Arguments).Contains("--output");
        await Assert.That(result.Arguments).Contains("/path/to/output");
    }

    [Test]
    public async Task Build_RuntimeIdentity_Overrides_StaticIdentity()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestAttributeOptions
        {
            Tool = "runtime-tool",
            CommandParts = ["runtime", "command"],
        });

        await Assert.That(result.ToString()).IsEqualTo("runtime-tool runtime command");
    }

    [Test]
    public async Task Build_PreferredAlias_Overrides_Subcommand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestAliasOptions());

        await Assert.That(result.ToString()).IsEqualTo("mytool short");
    }

    [Test]
    public async Task Build_Uses_Constructor_Computed_CommandParts()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestComputedCommandPartsOptions("deploy")
        {
            Force = true,
        });

        await Assert.That(result.ToString()).IsEqualTo("mytool resource deploy --force");
    }

    [Test]
    public async Task Build_WithPositionalArguments_PlacesCorrectly()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new TestPositionalOptions
        {
            FilePath = "test.cs",
            ConfigPath = "config.json"
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("processor");

        // The file path should appear before the config path based on placement
        var args = result.Arguments.ToList();
        var fileIndex = args.IndexOf("test.cs");
        var configIndex = args.IndexOf("config.json");

        await Assert.That(fileIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(configIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(fileIndex).IsLessThan(configIndex);
    }

    [Test]
    public async Task Build_ReturnsImmutableCommandLine()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new CommandLineToolOptions("echo")
        {
            Arguments = ["original"]
        };

        var result = builder.Build(options);

        // Verify the arguments are readonly
        await Assert.That(result.Arguments).IsTypeOf<System.Collections.ObjectModel.ReadOnlyCollection<string>>();
    }

    [Test]
    public async Task Build_ToString_FormatsCorrectly()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new CommandLineToolOptions("git")
        {
            Arguments = ["status", "-s"]
        };

        var result = builder.Build(options);

        await Assert.That(result.ToString()).IsEqualTo("git status -s");
    }

    [Test]
    public async Task Build_PreservesToolNameInArguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new CommandLineToolOptions("git")
        {
            Arguments = ["git", "status"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("git");
        await Assert.That(result.Arguments).IsEquivalentTo(
            ["git", "status"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Build_Places_Global_Options_Before_Subcommands()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var modelProvider = await GetService<ICommandModelProvider>();

        var searchPath = modelProvider.GetCommandModel(typeof(TestGlobalCommandOptions))
            .Single(part => part.PropertyName == nameof(TestGlobalOptions.SearchPath));
        await Assert.That(searchPath.IsGlobalOption).IsTrue();

        var result = builder.Build(new TestGlobalCommandOptions
        {
            SearchPath = "changelogs",
            ChangelogFile = "main.xml"
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "liquibase --search-path=changelogs update --changelog-file=main.xml");
    }

    [Test]
    public async Task Build_Places_Additional_Arguments_By_Phase_And_Scope()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestMultiLevelCommandOptions
        {
            Context = "remote",
            Reference = "build-reference",
            Follow = true,
            Arguments = ["manual"],
            AdditionalArguments =
            [
                new("--global-unmodeled", IsGlobalOption: true),
                new("early-unmodeled", CommandLinePhase.EarlyOperand),
                new("--normal-unmodeled"),
                new("pass-through", CommandLinePhase.Passthrough),
            ],
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "docker --global-unmodeled --context remote buildx history logs "
            + "early-unmodeled build-reference --normal-unmodeled --follow pass-through manual");
    }

    [Test]
    public async Task Build_Places_Additional_Option_Before_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            AdditionalArguments =
            [
                new("--unmodeled"),
            ],
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --unmodeled -- -1");
    }

    [Test]
    public async Task Build_Places_Additional_Terminal_Arguments_Last()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestAttributeOptions
        {
            Force = true,
            Arguments = ["manual"],
            AdditionalArguments =
            [
                new("terminal", CommandLinePhase.Terminal),
            ],
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "mytool sub command --force manual terminal");
    }

    [Test]
    public async Task Build_Rejects_Additional_Terminal_Argument_With_RunSettings()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestAttributeOptions
        {
            RunSettings = ["pass-through"],
            AdditionalArguments =
            [
                new("terminal", CommandLinePhase.Terminal),
            ],
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_Additional_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();
        CommandLinePhase[] phases =
        [
            CommandLinePhase.EarlyOperand,
            CommandLinePhase.Normal,
            CommandLinePhase.Passthrough,
            CommandLinePhase.Terminal,
        ];

        foreach (var phase in phases)
        {
            CommandLine Build() => builder.Build(new TestTerminalOptions
            {
                AdditionalArguments = [new("--", phase)],
                RunTests = "tests.jq",
            });

            await Assert.That(Build)
                .Throws<ArgumentException>()
                .And.HasMessageContaining("CliArgumentAttribute");
        }
    }

    [Test]
    public async Task Build_Keeps_MultiLevel_Command_Chain_Atomic()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestMultiLevelCommandOptions
        {
            Context = "remote",
            Reference = "build-reference",
            Follow = true,
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "docker --context remote buildx history logs build-reference --follow");
    }

    [CliTool("mytool")]
    [CliSubCommand("sub", "command")]
    internal record TestAttributeOptions : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public bool? Force { get; set; }

        [CliOption("--output")]
        public string? Output { get; set; }
    }

    [CliTool("tool")]
    private sealed record TestValidatedOptions : CommandLineToolOptions
    {
        [Display(Name = "Verbosity level")]
        [Range(0, 6)]
        [CliFlag("-v")]
        public int Verbose { get; init; }

        [RegularExpression("^[a-z]+$")]
        [CliOption("--name")]
        public string? Name { get; init; }

        [CliOptionValueRange(1, 3)]
        [CliOption("--level", ValueArity = CliOptionValueArity.Optional)]
        public CliOptionValue? Level { get; init; }
    }

    [CliTool("tool")]
    private sealed record TestRequiredShortCircuitOptions : CommandLineToolOptions
    {
        [ThrowingValidation]
        [Required]
        public string? Value { get; init; }
    }

    [CliTool("tool")]
    private sealed record TestMultipleRequiredOptions : CommandLineToolOptions
    {
        [CountingRequired]
        [CountingRequired]
        internal object Value { get; init; } = new();
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    private sealed class CountingRequiredAttribute : RequiredAttribute
    {
        private static readonly System.Runtime.CompilerServices.ConditionalWeakTable<object, InvocationCount>
            InvocationCounts = [];

        public CountingRequiredAttribute()
        {
            ErrorMessage = "Second required validator failed.";
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return false;
            }

            var count = InvocationCounts.GetValue(value, static _ => new InvocationCount());
            return Interlocked.Increment(ref count.Value) == 1;
        }

        private sealed class InvocationCount
        {
            public int Value;
        }
    }

    private sealed class ThrowingValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) =>
            throw new InvalidOperationException("Required validation should short-circuit this attribute.");
    }

    [CliTool("tool")]
    private sealed record TestNonPublicValidatedOptions : CommandLineToolOptions
    {
        [Range(0, 3)]
        [CliOption("--retries")]
        internal int Retries { get; init; }
    }

    [CliTool("tool")]
    private sealed record TestCollectedAndThrowingOptions : CommandLineToolOptions
    {
        [Range(1, 3)]
        internal int Retries { get; init; }

        [ThrowingValidation]
        public string Value { get; init; } = string.Empty;
    }

    [CliTool("tool")]
    private abstract record TestInheritedNonPublicValidatedOptionsBase : CommandLineToolOptions
    {
        [Range(0, 3)]
        [CliOption("--retries")]
        internal int Retries { get; init; }
    }

    private sealed record TestDerivedNonPublicValidatedOptions
        : TestInheritedNonPublicValidatedOptionsBase;

    [CliTool("tool")]
    private abstract record TestOverriddenNonPublicValidatedOptionsBase : CommandLineToolOptions
    {
        [Display(Name = "Retry count")]
        [Range(0, 3)]
        [CliOption("--retries")]
        internal virtual int Retries { get; init; }
    }

    private sealed record TestOverriddenNonPublicValidatedOptions
        : TestOverriddenNonPublicValidatedOptionsBase
    {
        internal override int Retries { get; init; }
    }

    [CliTool("tool")]
    private abstract record TestNonInheritedValidationOverrideOptionsBase : CommandLineToolOptions
    {
        [AlwaysInvalidNonInherited]
        [CliOption("--value")]
        internal virtual string Value => "value";
    }

    private sealed record TestNonInheritedValidationOverrideOptions
        : TestNonInheritedValidationOverrideOptionsBase
    {
        internal override string Value => "value";
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    private sealed class AlwaysInvalidNonInheritedAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => false;
    }

    [CliTool("tool")]
    private abstract record TestDerivedValidationOverrideOptionsBase : CommandLineToolOptions
    {
        [Range(0, 3)]
        [CliOption("--retries")]
        internal virtual int Retries { get; init; }
    }

    private sealed record TestDerivedValidationOverrideOptions
        : TestDerivedValidationOverrideOptionsBase
    {
        [Range(4, 6)]
        internal override int Retries { get; init; }
    }

    [CliTool("tool")]
    private record TestProtectedInternalValidatedOptions : CommandLineToolOptions
    {
        [Range(0, 3)]
        [CliOption("--retries")]
        protected internal int Retries { get; init; }
    }

    [CliTool("tool")]
    private record TestUnsupportedNonPublicValidatedOptions : CommandLineToolOptions
    {
        [Required]
        private string? PrivateHelper => null;

        [Required]
        protected string? ProtectedHelper => null;

        [Required]
        private protected string? PrivateProtectedHelper => null;
    }

    [CliTool("tool")]
    private sealed record TestNonPublicAndObjectValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        [Range(1, 3)]
        [CliOption("--retries")]
        internal int Retries { get; init; }

        [Required]
        [CliOption("--name")]
        public string? Name { get; init; }

        public bool ValidationCallbackInvoked { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidationCallbackInvoked = true;
            yield return new ValidationResult("Callback validation failed.");
        }
    }

    [TypeValidationTracker]
    [CliTool("tool")]
    private sealed record TestPropertyFirstValidatedOptions : CommandLineToolOptions
    {
        [Required]
        public string? Name { get; init; }

        [Range(1, 3)]
        internal int Retries { get; init; }

        public bool TypeValidationInvoked { get; private set; }

        public void MarkTypeValidationInvoked() => TypeValidationInvoked = true;
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class TypeValidationTrackerAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ((TestPropertyFirstValidatedOptions) value!).MarkTypeValidationInvoked();
            return new ValidationResult("Type validation should be skipped.");
        }
    }

    [CliTool("tool")]
    private sealed record TestSuccessfulObjectValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success!;
        }
    }

    [CliTool("tool")]
    private sealed record TestThrowingValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            throw new ValidationException("Callback validation failed.");
    }

    [CliTool("tool")]
    private sealed record TestServiceValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        [RequiresCommandLineBuilder]
        internal string Value { get; init; } = "valid";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (validationContext.GetService(typeof(ICommandLineBuilder)) is null)
            {
                yield return new ValidationResult("Scoped service unavailable to object validation.");
            }
        }
    }

    private sealed class RequiresCommandLineBuilderAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
            validationContext.GetService(typeof(ICommandLineBuilder)) is null
                ? new ValidationResult("Scoped service unavailable to property validation.")
                : ValidationResult.Success;
    }

    [CliTool("tool")]
    private sealed record TestSecretValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        [CliOption("--token")]
        [SecretValue]
        public string Token { get; init; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult($"Invalid token {Token}", [nameof(Token)]);
        }
    }

    [CliTool("tool")]
    private sealed record TestThrowingSecretValidatedOptions : CommandLineToolOptions, IValidatableObject
    {
        [CliOption("--token")]
        [SecretValue]
        public string Token { get; init; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            throw new InvalidOperationException($"Invalid token {Token}");
    }

    [CliTool("tool")]
    private sealed record TestSetterOnlyValidatedOptions : CommandLineToolOptions
    {
        [Required]
        public string? Value
        {
            set { }
        }
    }

    [CliTool("tool")]
    private sealed record TestTypeDescriptorValidatedOptions : CommandLineToolOptions
    {
        public string? Name { get; init; }
    }

    private sealed class TestTypeDescriptorValidationMetadata
    {
        [Required]
        public string? Name { get; init; }
    }

    [CliTool("tool")]
    private sealed record TestProviderExposedNonPublicOptions : CommandLineToolOptions, ICustomTypeDescriptor
    {
        private static readonly PropertyInfo RetriesProperty = typeof(TestProviderExposedNonPublicOptions)
            .GetProperty(nameof(Retries), BindingFlags.Instance | BindingFlags.NonPublic)!;
        private static readonly PropertyInfo NameProperty = typeof(TestProviderExposedNonPublicOptions)
            .GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.NonPublic)!;
        private static readonly PropertyInfo RequiredValueProperty = typeof(TestProviderExposedNonPublicOptions)
            .GetProperty(nameof(RequiredValue), BindingFlags.Instance | BindingFlags.NonPublic)!;
        private static readonly PropertyDescriptorCollection Properties =
            new(
                [
                    new NonPublicPropertyDescriptor(RetriesProperty),
                    new NonPublicPropertyDescriptor(NameProperty, [new RequiredAttribute()]),
                    new NonPublicPropertyDescriptor(
                        RequiredValueProperty,
                        [new NullIntolerantValidationAttribute()]),
                ],
                readOnly: true);

        [CountingValidation]
        internal int Retries { get; init; }

        [StringLength(10)]
        internal string? Name { get; init; } = "valid";

        [Required]
        internal string? RequiredValue { get; init; } = "valid";

        public bool RejectRetries { get; init; } = true;

        public int ValidationCount { get; private set; }

        public int ProviderValidationCount { get; private set; }

        public void RecordValidation() => ValidationCount++;

        public void RecordProviderValidation() => ProviderValidationCount++;

        AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;

        string? ICustomTypeDescriptor.GetClassName() => null;

        string? ICustomTypeDescriptor.GetComponentName() => null;

        TypeConverter? ICustomTypeDescriptor.GetConverter() => null;

        EventDescriptor? ICustomTypeDescriptor.GetDefaultEvent() => null;

        PropertyDescriptor? ICustomTypeDescriptor.GetDefaultProperty() => null;

        object? ICustomTypeDescriptor.GetEditor(Type editorBaseType) => null;

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => EventDescriptorCollection.Empty;

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[]? attributes) =>
            EventDescriptorCollection.Empty;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => Properties;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[]? attributes) => Properties;

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor? propertyDescriptor) => this;
    }

    private sealed class CountingValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var options = (TestProviderExposedNonPublicOptions) validationContext.ObjectInstance;
            options.RecordValidation();
            return options.RejectRetries
                ? new ValidationResult("Retries are invalid.")
                : ValidationResult.Success;
        }
    }

    private sealed class NullIntolerantValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var options = (TestProviderExposedNonPublicOptions) validationContext.ObjectInstance;
            options.RecordProviderValidation();
            return value is null
                ? throw new InvalidOperationException("Provider validator must not receive a missing required value.")
                : ValidationResult.Success;
        }
    }

    private sealed class NonPublicPropertyDescriptor(
        PropertyInfo property,
        Attribute[]? additionalAttributes = null)
        : PropertyDescriptor(
            property.Name,
            [
                .. property.GetCustomAttributes<Attribute>(inherit: true),
                .. additionalAttributes ?? [],
            ])
    {
        public override Type ComponentType => property.DeclaringType!;

        public override bool IsReadOnly => true;

        public override Type PropertyType => property.PropertyType;

        public override bool CanResetValue(object component) => false;

        public override object? GetValue(object? component) => property.GetValue(component);

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object? component, object? value) =>
            throw new NotSupportedException();

        public override bool ShouldSerializeValue(object component) => false;
    }

    [CliTool("mytool")]
    internal sealed record TestComputedCommandPartsOptions : CommandLineToolOptions
    {
        public TestComputedCommandPartsOptions(string action)
        {
            CommandParts = ["resource", action];
        }

        [CliFlag("--force")]
        public bool? Force { get; init; }
    }

    [CliTool("processor")]
    internal record TestPositionalOptions : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? FilePath { get; set; }

        [CliArgument(1, Phase = CommandLinePhase.Passthrough)]
        public string? ConfigPath { get; set; }
    }

    [CliTool("tool")]
    private sealed record TestNegatedManualFlagOptions : CommandLineToolOptions
    {
        [CliFlag("--feature", NegatedName = "--no-feature")]
        public bool? Feature { get; init; }

        [CliArgument(0, PrependOptionTerminator = true)]
        public string? Filter { get; init; }
    }

    [CliTool("jq")]
    internal record TestTerminalOptions : CommandLineToolOptions
    {
        [CliOption("--arg")]
        public CliValuePair? Argument { get; set; }

        [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
        public string? Color { get; set; }

        [CliOption("--define", Format = OptionFormat.ColonSeparated)]
        public string? Define { get; set; }

        [CliOption("--variable", Format = OptionFormat.NoSeparator)]
        public string? Variable { get; set; }

        [CliOption("-D", Format = OptionFormat.NoSeparator)]
        public string? ShortDefine { get; set; }

        [CliOption("-Debug")]
        public string? DebugValue { get; set; }

        [CliFlag("--compact", ShortForm = "-c")]
        public bool? Compact { get; set; }

        [CliOption("--dry-run", ValueArity = CliOptionValueArity.Optional)]
        public CliOptionValue? DryRun { get; set; }

        [CliOption("--arguments", GroupValues = true)]
        public IReadOnlyList<string>? GroupedArguments { get; set; }

        [CliArgument(0, PrependOptionTerminator = true)]
        public string? Filter { get; set; }

        [CliOption(
            "--run-tests",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal)]
        public CliOptionValue? RunTests { get; set; }

        [CliFlag("--terminal-flag", ShortForm = "-T", Phase = CommandLinePhase.Terminal)]
        public bool? TerminalFlag { get; set; }

        [CliArgument(1, Phase = CommandLinePhase.Terminal, PrependOptionTerminator = true)]
        public string? TerminalArgument { get; set; }
    }

    [CliTool("pulumi")]
    [CliSubCommand("package", "info")]
    private sealed record TestEarlyOperandTerminatorOptions(
        [property: CliArgument(0, Phase = CommandLinePhase.EarlyOperand)] string Provider)
        : CommandLineToolOptions
    {
        [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
        public string? Color { get; init; }

        [CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true)]
        public IReadOnlyList<string>? Parameters { get; init; }
    }

    [CliTool("tool")]
    [CliSubCommand("copy")]
    private sealed record TestTerminatedPassthroughOptions(
        [property: CliArgument(0, Phase = CommandLinePhase.LateOperand, Required = true)] string Source,
        [property: CliArgument(1, Phase = CommandLinePhase.LateOperand, Required = true)] string Destination)
        : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public bool Force { get; init; }

        [CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true)]
        public IReadOnlyList<string>? ScpFlags { get; init; }
    }

    [CliTool("tool")]
    [CliSubCommand("run")]
    private sealed record TestRequiredOperandCompatibilityOptions(
        [property: CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)] string Operand)
        : CommandLineToolOptions
    {
        public TestRequiredOperandCompatibilityOptions()
            : this(default(string)!)
        {
        }
    }

    [CliTool("test")]
    private sealed record TestRequiredSinglePassOperandOptions(IEnumerable<string> values)
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

    [CliTool("test")]
    private sealed record TestRequiredEarlyOperandOptions : CommandLineToolOptions
    {
        [CliArgument(
            0,
            Phase = CommandLinePhase.EarlyOperand,
            Required = true,
            PrependOptionTerminatorIfValueStartsWithDash = true)]
        public string? Operand { get; init; }

        [CliFlag("--force")]
        public bool Force { get; init; }
    }

    [CliTool("dotnet")]
    [CliSubCommand("test")]
    private sealed record TestRepeatedOptionTerminatorOptions : CommandLineToolOptions
    {
        [CliArgument(0, PrependOptionTerminator = true)]
        public IReadOnlyList<string>? PlatformOptions { get; init; }

        [CliArgument(1, PrependOptionTerminator = true, RepeatOptionTerminator = true)]
        public IReadOnlyList<string>? ExtensionOptions { get; init; }
    }

    [CliTool("dotnet")]
    private sealed record TestMultiCharacterShortOptionOptions : CommandLineToolOptions
    {
        [CliOption("--source", ShortForm = "-s")]
        public string? Source { get; init; }

        [CliOption("--symbol-source", ShortForm = "-ss")]
        public string? SymbolSource { get; init; }
    }

    private sealed record DerivedCliValuePair(string First, string Second)
        : CliValuePair(First, Second);

    [CliTool("jq")]
    private sealed record ReflectionDerivedPairOptions<T> : CommandLineToolOptions
    {
        [CliOption("--arg")]
        public T? Pair { get; init; }
    }

    [CliTool("tool")]
    private sealed record TestZeroManualOperandOptions : CommandLineToolOptions;

    [CliTool("jq")]
    private record TestManualTerminatorOptions : CommandLineToolOptions
    {
        [CliFlag("--compact-output")]
        public bool? CompactOutput { get; set; }

        [CliArgument(0)]
        public string? Filter { get; set; }
    }

    [CliTool("mytool")]
    [CliSubCommand("long", "command")]
    [CliCommandAlias("short", IsPreferred = true)]
    internal record TestAliasOptions : CommandLineToolOptions;

    [CliTool("liquibase")]
    [CliGlobalOptions]
    internal abstract record TestGlobalOptions : CommandLineToolOptions
    {
        [CliFlag("--verbose", ShortForm = "-v")]
        public bool? Verbose { get; set; }

        [CliOption("--search-path", Format = OptionFormat.EqualsSeparated)]
        public string? SearchPath { get; set; }
    }

    [CliSubCommand("update")]
    internal sealed record TestGlobalCommandOptions : TestGlobalOptions
    {
        [CliFlag("--force", ShortForm = "-f")]
        public bool? Force { get; set; }

        [CliOption("--changelog-file", Format = OptionFormat.EqualsSeparated)]
        public string? ChangelogFile { get; set; }

        [CliOption("--set")]
        public string? Set { get; set; }

        [CliArgument(0, PrependOptionTerminatorIfValueStartsWithDash = true)]
        public string? Input { get; set; }
    }

    [CliTool("mytool")]
    [CliGlobalOptions]
    private abstract record TestGlobalTerminatorOptions : CommandLineToolOptions
    {
        [CliArgument(0, PrependOptionTerminatorIfValueStartsWithDash = true)]
        public string? GlobalOperand { get; set; }
    }

    [CliSubCommand("run")]
    private sealed record TestGlobalTerminatorCommandOptions : TestGlobalTerminatorOptions
    {
        [CliFlag("--force")]
        public bool Force { get; set; }
    }

    [CliTool("docker")]
    [CliGlobalOptions]
    internal abstract record TestMultiLevelGlobalOptions : CommandLineToolOptions
    {
        [CliOption("--context")]
        public string? Context { get; set; }
    }

    [CliSubCommand("buildx", "history", "logs")]
    internal sealed record TestMultiLevelCommandOptions : TestMultiLevelGlobalOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? Reference { get; set; }

        [CliFlag("--follow")]
        public bool? Follow { get; set; }
    }
}
