using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
            await Assert.That(diagnostic.Id).IsEqualTo("MPGEN002");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
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

    private static GeneratorDriverRunResult RunGenerator(string source)
    {
        var infrastructureSyntaxTree = CSharpSyntaxTree.ParseText(TestInfrastructure);
        var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [infrastructureSyntaxTree, sourceSyntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var compilationErrors = compilation.GetDiagnostics()
            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        if (compilationErrors.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, compilationErrors));
        }

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new ModuleExtensionsGenerator());

        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out _,
            out _);

        return driver.GetRunResult();
    }
}
