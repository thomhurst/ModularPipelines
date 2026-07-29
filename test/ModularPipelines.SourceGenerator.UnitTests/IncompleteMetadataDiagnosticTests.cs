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
    public async Task Inaccessible_Command_Options_Type_Reports_Informational_Skip()
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
            "global::Container.HiddenOptions");
    }

    [Test]
    public async Task Inaccessible_Secret_Type_Reports_Informational_Skip()
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
            "global::Container.HiddenSecrets");
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
            "global::GenericOptions<T>");
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

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo(diagnosticId);
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Warning);
            await Assert.That(diagnostic.GetMessage()).Contains(typeName);
            await Assert.That(diagnostic.GetMessage()).Contains("runtime reflection");
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith($"#{diagnosticId.ToLowerInvariant()}");
            await Assert.That(diagnostic.Location.IsInSource).IsTrue();
            await Assert.That(result.GeneratedTrees).HasSingleItem();
        }
    }

    private static async Task AssertSkippedDiagnostic(
        GeneratorDriverRunResult result,
        string diagnosticId,
        string typeName)
    {
        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo(diagnosticId);
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Info);
            await Assert.That(diagnostic.GetMessage()).Contains(typeName);
            await Assert.That(diagnostic.GetMessage()).Contains("runtime reflection");
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith($"#{diagnosticId.ToLowerInvariant()}");
            await Assert.That(diagnostic.Location.IsInSource).IsTrue();
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }
}
