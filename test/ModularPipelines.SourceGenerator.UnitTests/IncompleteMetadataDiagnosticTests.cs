using Microsoft.CodeAnalysis;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class IncompleteMetadataDiagnosticTests
{
    private const string CommandInfrastructure = """
        namespace ModularPipelines.Options
        {
            public abstract class CommandLineToolOptions;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class CliOptionAttribute(string name) : System.Attribute;

            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class CliFlagAttribute(string name) : System.Attribute;

            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class SecretValueAttribute(params string[] keys) : System.Attribute;
        }
        """;

    private const string ModuleInfrastructure = """
        namespace ModularPipelines.Modules
        {
            public abstract class Module<T>;
        }
        """;

    private const string OptionsRegistrationInfrastructure = CommandInfrastructure + """

        namespace Microsoft.Extensions.Options
        {
            public sealed class OptionsBuilder<TOptions>;

            public interface IOptions<out TOptions>;

            public static class Options
            {
                public static IOptions<TOptions> Create<TOptions>(TOptions options) => null!;
            }
        }

        namespace Microsoft.Extensions.DependencyInjection
        {
            public interface IServiceCollection;

            public sealed class ServiceCollection : IServiceCollection;

            public static class OptionsServiceCollectionExtensions
            {
                public static IServiceCollection Configure<TOptions>(
                    this IServiceCollection services,
                    System.Action<TOptions> configureOptions) => services;

                public static object AddOptions<TOptions>(
                    this IServiceCollection services) => new object();
            }

            public static class ServiceCollectionServiceExtensions
            {
                public static IServiceCollection AddSingleton<TService>(
                    this IServiceCollection services,
                    TService implementation) => services;
            }
        }
        """;

    [Test]
    public async Task Inaccessible_Command_Property_Reports_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class TestOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                private string Value { get; } = "";
            }
            """);

        await AssertIncompleteDiagnostic(
            result,
            "MPG0003",
            "global::TestOptions");
        await Assert.That(result.Diagnostics.Single().GetMessage()).Contains("Value");
    }

    [Test]
    public async Task Null_Command_Attribute_Names_Report_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class TestOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliFlag(null!)]
                public bool Flag { get; } = false;

                [ModularPipelines.Attributes.CliOption(null!)]
                public string Option { get; } = "";
            }
            """);

        await AssertIncompleteDiagnostic(
            result,
            "MPG0003",
            "global::TestOptions");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(generatedSource).DoesNotContain("FlagPart");
            await Assert.That(generatedSource).DoesNotContain("OptionPart");
            await Assert.That(generatedSource).DoesNotContain(
                "GeneratedCommandMetadata.Register(\n            typeof(global::TestOptions)");
        }
    }

    [Test]
    public async Task Obsolete_Error_Command_Property_Reports_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class TestOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [System.Obsolete("Removed", true)]
                [ModularPipelines.Attributes.CliOption("--legacy")]
                public string Legacy { get; } = "";
            }
            """);

        await AssertIncompleteDiagnostic(result, "MPG0003", "global::TestOptions");
        await Assert.That(result.Diagnostics.Single().GetMessage()).Contains("Legacy");
        await Assert.That(result.GeneratedTrees.Single().ToString())
            .DoesNotContain("instance).@Legacy");
    }

    [Test]
    public async Task Obsolete_Error_Secret_Property_Reports_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class TestOptions
            {
                [System.Obsolete("Removed", true)]
                [ModularPipelines.Attributes.SecretValue]
                public string Legacy { get; } = "";
            }
            """);

        await AssertIncompleteDiagnostic(result, "MPG0004", "global::TestOptions");
        await Assert.That(result.Diagnostics.Single().GetMessage()).Contains("Legacy");
        await Assert.That(result.GeneratedTrees.Single().ToString())
            .DoesNotContain("instance).@Legacy");
    }

    [Test]
    public async Task Friend_Assembly_Properties_Are_Accessible()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("GeneratorTests")]

            namespace External;

            public abstract class FriendOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                internal string Value { get; } = "";

                [ModularPipelines.Attributes.SecretValue]
                internal string Token { get; } = "";
            }
            """,
            """
            public sealed class TestOptions : External.FriendOptions;
            """);

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("OptionPart");
            await Assert.That(generatedSource).Contains("new(\"Token\"");
            await Assert.That(generatedSource).Contains("GeneratedSecretMetadata.Register");
        }
    }

    [Test]
    public async Task Trimmed_Host_Generates_Metadata_For_Unprocessed_Referenced_Options()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace External;

            public sealed class CrossLanguageOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                public string Value { get; } = "";

                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }
            """,
            "public sealed class TrimmedHost;",
            new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("GeneratedCommandMetadata.RegisterExternal(");
            await Assert.That(generatedSource).Contains("typeof(global::External.CrossLanguageOptions)");
            await Assert.That(generatedSource).Contains("OptionPart");
            await Assert.That(generatedSource).Contains("GeneratedSecretMetadata.RegisterExternal(");
            await Assert.That(generatedSource).Contains("new(\"Token\"");
        }
    }

    [Test]
    public async Task Trimmed_Host_Suppresses_External_Experimental_Diagnostics()
    {
        var (result, compilationDiagnostics) =
            GeneratorTestHarness.RunWithExternalAssemblyAndGetCompilationDiagnostics(
                new CommandOptionsGenerator(),
                CommandInfrastructure,
                """
                using System.Diagnostics.CodeAnalysis;

                [assembly: Experimental("LIBASSEMBLY001")]

                namespace External;

                [Experimental("LIBPROPERTYTYPE001")]
                public sealed class ExperimentalValue;

                [Experimental("LIBBASE001")]
                public class ExperimentalBaseOptions
                    : ModularPipelines.Options.CommandLineToolOptions
                {
                    [Experimental("LIBPROPERTY001")]
                    [ModularPipelines.Attributes.CliOption("--value")]
                    public System.Collections.Generic.List<ExperimentalValue[]> Value { get; } = [];
                }

                [Experimental("LIBOUTER001")]
                public static class ExperimentalContainer
                {
                    [Experimental("LIBTYPE001")]
                    public sealed class ExperimentalOptions
                        : ExperimentalBaseOptions;
                }
                """,
                "public sealed class TrimmedHost;",
                new Dictionary<string, string>
                {
                    ["build_property.PublishTrimmed"] = "true",
                });

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(compilationDiagnostics
                    .Where(static diagnostic => diagnostic.Id is
                        "LIBASSEMBLY001" or "LIBBASE001" or "LIBOUTER001"
                        or "LIBPROPERTY001" or "LIBPROPERTYTYPE001" or "LIBTYPE001"))
                .IsEmpty();
            await Assert.That(generatedSource).Contains(
                "#pragma warning disable CS0612, CS0618, LIBASSEMBLY001, LIBBASE001, LIBOUTER001, LIBPROPERTY001, LIBPROPERTYTYPE001, LIBTYPE001");
            await Assert.That(generatedSource).Contains(
                "typeof(global::External.ExperimentalContainer.ExperimentalOptions)");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rescans_Legacy_Metadata_Marker()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace ModularPipelines.Generated
            {
                internal static class RuntimeMetadataRegistration;
            }

            namespace External
            {
                public class LegacyOptions
                    : ModularPipelines.Options.CommandLineToolOptions
                {
                    [ModularPipelines.Attributes.SecretValue]
                    protected string Token { get; } = "";
                }
            }
            """,
            "public sealed class TrimmedHost;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = result.Diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0004");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage()).Contains("global::External.LegacyOptions");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rescans_Current_Metadata_Marker()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace ModularPipelines.Generated
            {
                internal static class RuntimeMetadataRegistration
                {
                    public const int SchemaVersion = 1;
                }
            }

            namespace External
            {
                public class CurrentOptions
                    : ModularPipelines.Options.CommandLineToolOptions
                {
                    [ModularPipelines.Attributes.SecretValue]
                    protected string Token { get; } = "";
                }
            }
            """,
            "public sealed class TrimmedHost;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = result.Diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0004");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage()).Contains("global::External.CurrentOptions");
        }
    }

    [Test]
    public async Task Trimmed_Host_Generates_Metadata_For_Indirectly_Derived_Options()
    {
        var result = GeneratorTestHarness.RunWithIndirectExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace External;

            public class BaseOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }
            """,
            """
            namespace External;

            public sealed class LeafOptions : BaseOptions;
            """,
            "public sealed class TrimmedHost;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("typeof(global::External.LeafOptions)");
            await Assert.That(generatedSource).Contains("new(\"Token\"");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Colliding_Source_And_External_Type_Names()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace External;

            public sealed class CollidingOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.SecretValue]
                public string ExternalToken { get; } = "";
            }
            """,
            """
            namespace External;

            public sealed class CollidingOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.SecretValue]
                public string SourceToken { get; } = "";
            }
            """,
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = result.Diagnostics.Single();
        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0006");
            await Assert.That(diagnostic.GetMessage()).Contains("global::External.CollidingOptions");
            await Assert.That(generatedSource).DoesNotContain("global::External.CollidingOptions");
        }
    }

    [Test]
    public async Task Accessible_Type_Without_Secrets_Registers_Name_Based_Empty_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "public sealed class PlainOptions;");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).DoesNotContain("typeof(global::PlainOptions)");
            await Assert.That(generatedSource).Contains("GeneratedSecretMetadata.RegisterCoveredTypeName");
            await Assert.That(generatedSource).Contains("\"PlainOptions\"");
            await Assert.That(generatedSource).Contains("RegisterAssembly");
        }
    }

    [Test]
    public async Task Generated_Runtime_Metadata_Allows_Obsolete_Types()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "[System.Obsolete] public sealed class LegacyOptions;");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("#pragma warning disable CS0612, CS0618");
            await Assert.That(generatedSource).DoesNotContain("typeof(global::LegacyOptions)");
            await Assert.That(generatedSource).Contains("\"LegacyOptions\"");
        }
    }

    [Test]
    public async Task Obsolete_Error_Type_Uses_Name_Based_Coverage()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "[System.Obsolete(\"Removed\", true)] public sealed class LegacyOptions;");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).DoesNotContain("typeof(global::LegacyOptions)");
            await Assert.That(generatedSource).Contains("\"LegacyOptions\"");
        }
    }

    [Test]
    public async Task Nested_Type_In_Generic_Container_Uses_Name_Based_Coverage()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "public class Outer<T> { public sealed class Settings; }");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).DoesNotContain("typeof(global::Outer<T>.Settings)");
            await Assert.That(generatedSource).Contains("Outer`1+Settings");
        }
    }

    [Test]
    public async Task Delegate_Type_Registers_Exact_Empty_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "public delegate void Callback(string value);");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.RegisterCoveredTypeName");
            await Assert.That(generatedSource).Contains("\"Callback\"");
        }
    }

    [Test]
    public async Task Value_Types_Register_Exact_Empty_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public struct PlainStruct;
            public enum PlainEnum { Value }
            public readonly record struct PlainRecordStruct;
            """);

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("\"PlainStruct\"");
            await Assert.That(generatedSource).Contains("\"PlainEnum\"");
            await Assert.That(generatedSource).Contains("\"PlainRecordStruct\"");
        }
    }

    [Test]
    public async Task Inaccessible_Secret_Property_Reports_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class Secrets
            {
                [ModularPipelines.Attributes.SecretValue]
                private string Token { get; } = "";
            }
            """);

        await AssertIncompleteDiagnostic(
            result,
            "MPG0004",
            "global::Secrets");
        await Assert.That(result.Diagnostics.Single().GetMessage()).Contains("Token");
    }

    [Test]
    public async Task Equivalent_Secret_Compilation_Uses_Incremental_Cache()
    {
        var result = GeneratorTestHarness.RunTwiceWithStepTracking(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class Secrets
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }
            """);

        await Assert.That(GeneratorTestHarness.HasCachedOrUnchangedOutput(result)).IsTrue();
    }

    [Test]
    public async Task Inaccessible_Module_Attribute_Reports_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            ModuleInfrastructure,
            """
            public class Container
            {
                [System.AttributeUsage(System.AttributeTargets.Class)]
                private sealed class HiddenAttribute : System.Attribute;

                [Hidden]
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        await AssertIncompleteDiagnostic(
            result,
            "MPG0005",
            "global::Container.BuildModule");
    }

    [Test]
    public async Task Inaccessible_Command_Options_Type_Reports_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public class Container
            {
                private sealed class HiddenOptions
                    : ModularPipelines.Options.CommandLineToolOptions;
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::Container.HiddenOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Inaccessible_Secret_Type_Reports_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public class Container
            {
                private sealed class HiddenSecrets
                {
                    [ModularPipelines.Attributes.SecretValue]
                    public string Token { get; } = "";
                }
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::Container.HiddenSecrets",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Generic_Type_Inheriting_Secret_Reports_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public class SecretBase
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }

            public sealed class GenericOptions<T> : SecretBase;
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::GenericOptions<T>",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Generic_Command_Options_Type_Reports_Skipped_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public sealed class GenericOptions<T>
                : ModularPipelines.Options.CommandLineToolOptions;
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::GenericOptions<T>",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Abstract_Generic_Command_Options_Are_Ignored()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public abstract class GenericOptions<T>
                : ModularPipelines.Options.CommandLineToolOptions;
            """);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(result.GeneratedTrees.Single().ToString()).DoesNotContain("GenericOptions");
        }
    }

    [Test]
    public async Task Hidden_Base_Secret_Uses_Base_Accessor()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public class SecretBase
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "base";
            }

            public sealed class DerivedSecrets : SecretBase
            {
                public new int Token { get; } = 42;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains(
                "static instance => ((global::SecretBase)instance).@Token");
            await Assert.That(generatedSource).DoesNotContain(
                "static instance => ((global::DerivedSecrets)instance).@Token");
        }
    }

    [Test]
    public async Task Single_Declaration_Partial_Secret_Type_Reports_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialSecrets
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialSecrets",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Single_Declaration_Partial_Unannotated_Type_Registers_Incomplete_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialOptions;
            """);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            var generatedSource = result.GeneratedTrees.Single().ToString();
            await Assert.That(generatedSource).Contains("RegisterIncompleteTypeNames");
            await Assert.That(generatedSource).Contains(
                "[assembly: global::ModularPipelines.Generated.IncompleteRuntimeMetadataAttribute(\"PartialOptions\")]");
            await Assert.That(generatedSource).Contains("\"PartialOptions\"");
        }
    }

    [Test]
    public async Task Trimmed_Host_Allows_Unrelated_Partial_Type()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "public partial class Program;",
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        await Assert.That(result.Diagnostics).IsEmpty();
    }

    [Test]
    public async Task Aot_Host_Rejects_Partial_Unannotated_Options_Type()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace Microsoft.Extensions.Options
            {
                public interface IOptions<out TOptions>
                {
                    TOptions Value { get; }
                }
            }

            public partial class PartialOptions;

            public sealed class Consumer(
                Microsoft.Extensions.Options.IOptions<PartialOptions> options);
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Partial_Unannotated_Options_Type()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            namespace Microsoft.Extensions.Options
            {
                public interface IOptions<out TOptions>
                {
                    TOptions Value { get; }
                }
            }

            public partial class PartialOptions;

            public sealed class Consumer(
                Microsoft.Extensions.Options.IOptions<PartialOptions> options);
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Aot_Host_Rejects_Partial_Options_Registered_With_Configure()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            using Microsoft.Extensions.DependencyInjection;

            public partial class PartialOptions;

            public static class Registration
            {
                public static void Add(IServiceCollection services) =>
                    services.Configure<PartialOptions>(_ => { });
            }
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Jit_Host_Rejects_Partial_Options_Registered_With_Configure()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            using Microsoft.Extensions.DependencyInjection;

            public partial class PartialOptions;

            public static class Registration
            {
                public static void Add(IServiceCollection services) =>
                    services.Configure<PartialOptions>(_ => { });
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Jit_Host_Rejects_Partial_Options_Exposed_Through_OptionsBuilder()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            public partial class PartialOptions;

            public static class Registration
            {
                public static void Configure(
                    Microsoft.Extensions.Options.OptionsBuilder<PartialOptions> builder)
                {
                }
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Jit_Host_Rejects_Inferred_Partial_Options_Registration()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.Options;

            public partial class PartialOptions;

            public static class Registration
            {
                public static void Add(IServiceCollection services) =>
                    services.AddSingleton(Options.Create(new PartialOptions()));
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Jit_Host_Rejects_External_Partial_Options_Usage()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            [assembly: ModularPipelines.Generated.IncompleteRuntimeMetadataAttribute(
                "External.PartialOptions")]

            namespace ModularPipelines.Generated
            {
                [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = true)]
                internal sealed class IncompleteRuntimeMetadataAttribute(string metadataName)
                    : System.Attribute;
            }

            namespace External
            {
                public partial class PartialOptions;

                public sealed class RuntimeReference
                    : ModularPipelines.Options.CommandLineToolOptions;
            }
            """,
            """
            public sealed class Consumer(
                Microsoft.Extensions.Options.IOptions<External.PartialOptions> options);
            """);

        var diagnostic = result.Diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0006");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage()).Contains("global::External.PartialOptions");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Partial_Options_Registered_With_AddOptions()
    {
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            OptionsRegistrationInfrastructure,
            """
            using Microsoft.Extensions.DependencyInjection;

            public partial class PartialOptions;

            public static class Registration
            {
                public static void Add(IServiceCollection services) =>
                    services.AddOptions<PartialOptions>();
            }
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Single_Declaration_Partial_Command_Options_Report_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                public string Value { get; } = "";
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Single_Declaration_Partial_Base_Rejects_Derived_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialBaseOptions
                : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                public string Value { get; } = "";

                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }

            public sealed class DerivedOptions : PartialBaseOptions;
            """);

        var messages = result.Diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToList();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).Count().IsEqualTo(2);
            await Assert.That(result.Diagnostics.All(diagnostic => diagnostic.Id == "MPG0006")).IsTrue();
            await Assert.That(messages.Any(message => message.Contains("global::PartialBaseOptions"))).IsTrue();
            await Assert.That(messages.Any(message => message.Contains("global::DerivedOptions"))).IsTrue();
        }
    }

    [Test]
    public async Task Split_Partial_Command_Options_Report_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialOptions
                : ModularPipelines.Options.CommandLineToolOptions;

            public partial class PartialOptions
            {
                [ModularPipelines.Attributes.CliOption("--value")]
                public string Value { get; } = "";
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialOptions",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Split_Partial_Secret_Type_Reports_Error()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialSecrets;

            public partial class PartialSecrets
            {
                [ModularPipelines.Attributes.SecretValue]
                public string Token { get; } = "";
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0006",
            "global::PartialSecrets",
            DiagnosticSeverity.Error);
    }

    [Test]
    public async Task Split_Partial_Unannotated_Type_Does_Not_Register_Complete_Coverage()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            public partial class PartialOptions;
            public partial class PartialOptions;
            """);

        await Assert.That(result.Diagnostics).IsEmpty();
        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(generatedSource).Contains("RegisterIncompleteTypeNames");
            await Assert.That(generatedSource).Contains("PartialOptions");
        }
    }

    [Test]
    public async Task File_Local_Type_Uses_Name_Based_Coverage()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            file sealed class FileOptions;
            """);

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("RegisterCoveredTypeName");
            await Assert.That(generatedSource).DoesNotContain("typeof(global::FileOptions)");
        }
    }

    [Test]
    public async Task Same_Named_File_Local_Types_Register_Separate_Coverage()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            """
            file sealed class FileOptions;
            """,
            """
            file sealed class FileOptions;
            """);

        var generatedSource = result.GeneratedTrees.Single().ToString();
        var coveredNames = generatedSource.Split(
            "FileOptions",
            StringSplitOptions.None).Length - 1;

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("RegisterCoveredTypeNames");
            await Assert.That(coveredNames).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Inaccessible_Module_Type_Reports_Informational_Skip()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            ModuleInfrastructure,
            """
            public class Container
            {
                private sealed class HiddenModule
                    : ModularPipelines.Modules.Module<string>;
            }
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0007",
            "global::Container.HiddenModule");
    }

    [Test]
    public async Task Generic_Module_Type_Reports_Skipped_Diagnostic()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            ModuleInfrastructure,
            """
            public sealed class GenericModule<T>
                : ModularPipelines.Modules.Module<T>;
            """);

        await AssertSkippedDiagnostic(
            result,
            "MPG0007",
            "global::GenericModule<T>");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Incremental_Diagnostic_Location_Tracks_Source_Edit(bool moduleMetadata)
    {
        var infrastructure = moduleMetadata ? ModuleInfrastructure : CommandInfrastructure;
        var candidate = moduleMetadata
            ? """
              public class Container
              {
                  [System.AttributeUsage(System.AttributeTargets.Class)]
                  private sealed class HiddenAttribute : System.Attribute;

                  [Hidden]
                  public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
              }
              """
            : """
              public sealed class TestOptions : ModularPipelines.Options.CommandLineToolOptions
              {
                  [ModularPipelines.Attributes.CliOption("--value")]
                  private string Value { get; } = "";
              }
              """;
        var updatedCandidate = $"{Environment.NewLine}{Environment.NewLine}{candidate}";
        var typeDeclaration = moduleMetadata
            ? "public sealed class BuildModule"
            : "public sealed class TestOptions";
        var expectedLine = updatedCandidate[..updatedCandidate.IndexOf(
                typeDeclaration,
                StringComparison.Ordinal)]
            .Count(static character => character == '\n');
        var generator = moduleMetadata
            ? (IIncrementalGenerator) new ModuleEventMetadataGenerator()
            : new CommandOptionsGenerator();

        var result = GeneratorTestRunner.RunIncrementalUpdate(
            generator,
            [infrastructure, candidate],
            [infrastructure, updatedCandidate]);
        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Location.SourceTree?.ToString())
                .IsEqualTo(updatedCandidate);
            await Assert.That(diagnostic.Location.GetLineSpan().StartLinePosition.Line)
                .IsEqualTo(expectedLine);
        }
    }

    [Test]
    public async Task Incremental_Partial_Aot_Diagnostic_Location_Tracks_Source_Edit()
    {
        const string candidate = """
            public partial class PartialOptions;

            public static class Registration
            {
                public static object Add(
                    Microsoft.Extensions.DependencyInjection.IServiceCollection services) =>
                    Microsoft.Extensions.DependencyInjection.OptionsServiceCollectionExtensions
                        .AddOptions<PartialOptions>(services);
            }
            """;
        var updatedCandidate = $"{Environment.NewLine}{Environment.NewLine}{candidate}";

        var result = GeneratorTestRunner.RunIncrementalUpdate(
            new CommandOptionsGenerator(),
            [OptionsRegistrationInfrastructure, candidate],
            [OptionsRegistrationInfrastructure, updatedCandidate],
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });
        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0006");
            await Assert.That(diagnostic.Location.SourceTree?.ToString())
                .IsEqualTo(updatedCandidate);
            await Assert.That(diagnostic.Location.GetLineSpan().StartLinePosition.Line)
                .IsEqualTo(2);
        }
    }

    private static async Task AssertIncompleteDiagnostic(
        GeneratorDriverRunResult result,
        string diagnosticId,
        string typeName)
    {
        var diagnostic = result.Diagnostics.Single();
        var requiresGeneratedMetadata = diagnosticId is "MPG0003" or "MPG0004";

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo(diagnosticId);
            await Assert.That(diagnostic.Severity).IsEqualTo(
                requiresGeneratedMetadata ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning);
            await Assert.That(diagnostic.GetMessage()).Contains(typeName);
            await Assert.That(diagnostic.GetMessage()).Contains(
                requiresGeneratedMetadata ? "accessible" : "runtime reflection");
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith($"#{diagnosticId.ToLowerInvariant()}");
            await Assert.That(diagnostic.Location.IsInSource).IsTrue();
            await Assert.That(result.GeneratedTrees).HasSingleItem();
        }
    }

    private static async Task AssertSkippedDiagnostic(
        GeneratorDriverRunResult result,
        string diagnosticId,
        string typeName,
        DiagnosticSeverity severity = DiagnosticSeverity.Info)
    {
        var diagnostic = result.Diagnostics.Single();
        var requiresGeneratedMetadata = diagnosticId == "MPG0006";

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo(diagnosticId);
            await Assert.That(diagnostic.Severity).IsEqualTo(severity);
            await Assert.That(diagnostic.GetMessage()).Contains(typeName);
            await Assert.That(diagnostic.GetMessage()).Contains(
                requiresGeneratedMetadata ? "accessible" : "runtime reflection");
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith($"#{diagnosticId.ToLowerInvariant()}");
            await Assert.That(diagnostic.Location.IsInSource).IsTrue();
            if (requiresGeneratedMetadata)
            {
                await Assert.That(result.GeneratedTrees).HasSingleItem();
            }
            else
            {
                await Assert.That(result.GeneratedTrees).IsEmpty();
            }
        }
    }
}
