using Microsoft.CodeAnalysis;

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
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }

            namespace Second
            {
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
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
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
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
                internal sealed class BuildModule : ModularPipelines.Modules.Module<string>;
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
                public sealed partial class BuildModule : ModularPipelines.Modules.Module<string>;
                public sealed partial class BuildModule : ModularPipelines.Modules.Module<string>;
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
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        await Assert.That(GeneratorTestHarness.HasCachedOrUnchangedOutput(result)).IsTrue();
    }
}
