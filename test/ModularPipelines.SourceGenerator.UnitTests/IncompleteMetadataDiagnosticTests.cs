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
    public async Task Accessible_Type_Without_Secrets_Registers_Exact_Empty_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            CommandInfrastructure,
            "public sealed class PlainOptions;");

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("typeof(global::PlainOptions)");
            await Assert.That(generatedSource).Contains("GeneratedSecretMetadata.Register");
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
            await Assert.That(generatedSource).Contains("#pragma warning disable CS0618");
            await Assert.That(generatedSource).Contains("typeof(global::LegacyOptions)");
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
                "GeneratedSecretMetadata.Register(typeof(global::Callback))");
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
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(typeof(global::PlainStruct))");
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(typeof(global::PlainEnum))");
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(typeof(global::PlainRecordStruct))");
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
    public async Task Single_Declaration_Partial_Secret_Type_Registers_Metadata()
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

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains("new(\"Token\"");
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(\n            typeof(global::PartialSecrets)");
        }
    }

    [Test]
    public async Task Single_Declaration_Partial_Unannotated_Type_Registers_Empty_Metadata()
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
            await Assert.That(result.GeneratedTrees.Single().ToString()).Contains(
                "GeneratedSecretMetadata.Register(typeof(global::PartialOptions))");
        }
    }

    [Test]
    public async Task Single_Declaration_Partial_Command_Options_Register_Metadata()
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

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains(
                "GeneratedCommandMetadata.Register(\n            typeof(global::PartialOptions)");
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(typeof(global::PartialOptions))");
        }
    }

    [Test]
    public async Task Single_Declaration_Partial_Base_Allows_Complete_Derived_Metadata()
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

        var generatedSource = result.GeneratedTrees.Single().ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).Contains(
                "GeneratedCommandMetadata.Register(\n            typeof(global::DerivedOptions)");
            await Assert.That(generatedSource).Contains(
                "GeneratedSecretMetadata.Register(\n            typeof(global::DerivedOptions)");
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
        await Assert.That(result.GeneratedTrees.Single().ToString()).DoesNotContain("PartialOptions");
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
        var registrations = generatedSource.Split(
            "RegisterCoveredTypeName",
            StringSplitOptions.None).Length - 1;

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(registrations).IsEqualTo(2);
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
