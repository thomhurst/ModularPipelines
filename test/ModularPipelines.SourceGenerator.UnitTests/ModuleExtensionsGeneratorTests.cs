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
        var result = RunGenerator("""
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
        var result = RunGenerator("""
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("public static global::Consumer.BuildModule GetBuildModule");
            await Assert.That(generatedSource)
                .Contains("context.GetModule<global::Consumer.BuildModule>()");
        }
    }

    [Test]
    public async Task Partial_Module_Declarations_Do_Not_Report_A_Collision()
    {
        var result = RunGenerator("""
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

    private static GeneratorDriverRunResult RunGenerator(string source)
    {
        return GeneratorTestRunner.Run(
            new ModuleExtensionsGenerator(),
            TestInfrastructure,
            source);
    }
}
