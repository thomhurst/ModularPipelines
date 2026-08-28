using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModuleExtensionsGeneratorTests
{
    private const string TestInfrastructure = """
        namespace ModularPipelines.Modules
        {
            public abstract class Module<T>;
        }
        """;

    [Test]
    public async Task Duplicate_Generated_Method_Names_Report_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleExtensionsGenerator(), TestInfrastructure, """
            namespace First
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }

            namespace Second
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0002");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith("#mpg0002");
            await Assert.That(diagnostic.GetMessage()).Contains("global::First.BuildModule");
            await Assert.That(diagnostic.GetMessage()).Contains("global::Second.BuildModule");
            await Assert.That(diagnostic.Location.IsInSource).IsTrue();
            await Assert.That(diagnostic.Location.GetLineSpan().StartLinePosition.Line).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Generated_Accessors_Use_Fully_Qualified_Module_Names()
    {
        var result = GeneratorTestHarness.Run(new ModuleExtensionsGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        await Assert.That(result.Diagnostics).IsEmpty();
        await SnapshotVerifier.VerifyAsync(
            "ModuleExtensionsGenerator.BuildModule",
            result.GeneratedTrees.Single().GetText().ToString());
    }

    [Test]
    public async Task Internal_Modules_Generate_Internal_Accessors()
    {
        var result = GeneratorTestHarness.Run(new ModuleExtensionsGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                internal sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generated)
                .Contains("internal static global::Consumer.BuildModule GetBuildModule(");
            await Assert.That(generated)
                .Contains("internal static global::Consumer.BuildModule? GetBuildModuleIfRegistered(");
        }
    }

    [Test]
    public async Task Partial_Module_Declarations_Do_Not_Report_A_Collision()
    {
        var result = GeneratorTestHarness.Run(new ModuleExtensionsGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed partial class BuildModule : ModularPipelines.Module<string>;
                public sealed partial class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(result.GeneratedTrees.Single().GetText().ToString())
            .Contains("GetBuildModule");
    }

    [Test]
    public async Task Equivalent_Compilation_Uses_Incremental_Cache()
    {
        var result = GeneratorTestHarness.RunTwiceWithStepTracking(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        await Assert.That(GeneratorTestHarness.HasCachedOrUnchangedOutput(result)).IsTrue();
    }

    [Test]
    public async Task Generated_Extension_Type_Is_Derived_From_Assembly_Name()
    {
        const string source = """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """;
        var sharedLibrary = GeneratorTestHarness.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source,
            "Shared.Modules");
        var pipelineApp = GeneratorTestHarness.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source,
            "9-Pipeline App");

        using (Assert.Multiple())
        {
            await Assert.That(GetGeneratedTypeName(sharedLibrary))
                .StartsWith("Shared_Modules_")
                .And.EndsWith("ModuleContextExtensions");
            await Assert.That(GetGeneratedTypeName(pipelineApp))
                .StartsWith("_9_Pipeline_App_")
                .And.EndsWith("ModuleContextExtensions");
        }
    }

    [Test]
    public async Task Assembly_Names_Produce_Unique_Extension_Types()
    {
        const string source = """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """;
        var hyphenated = GeneratorTestHarness.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source,
            "Shared-Modules");
        var dotted = GeneratorTestHarness.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source,
            "Shared.Modules");
        var craftedValidName = GetGeneratedTypeName(hyphenated)
            [..^"ModuleContextExtensions".Length];
        var valid = GeneratorTestHarness.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source,
            craftedValidName);

        using (Assert.Multiple())
        {
            await Assert.That(GetGeneratedTypeName(hyphenated))
                .IsNotEqualTo(GetGeneratedTypeName(dotted));
            await Assert.That(GetGeneratedTypeName(hyphenated))
                .IsNotEqualTo(GetGeneratedTypeName(valid));
        }
    }

    private static string GetGeneratedTypeName(GeneratorDriverRunResult result) =>
        result.GeneratedTrees
            .Single()
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Single()
            .Identifier
            .ValueText;
}
