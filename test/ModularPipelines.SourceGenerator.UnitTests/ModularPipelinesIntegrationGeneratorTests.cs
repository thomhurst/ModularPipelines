using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModularPipelinesIntegrationGeneratorTests
{
    private const string TestInfrastructure = """
        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Method)]
            public sealed class ModularPipelinesIntegrationAttribute : System.Attribute
            {
            }
        }

        namespace Microsoft.Extensions.DependencyInjection
        {
            public interface IServiceCollection
            {
            }
        }
        """;

    [Test]
    public async Task Invalid_Integration_Method_Reports_Diagnostic()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using Microsoft.Extensions.DependencyInjection;

            public class InvalidIntegration
            {
                [ModularPipelinesIntegration]
                public void Register(IServiceCollection services)
                {
                }
            }
            """);

        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPGEN001");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage()).Contains("InvalidIntegration.Register");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Valid_Integration_Method_Generates_Registrar()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using Microsoft.Extensions.DependencyInjection;

            public static class ValidIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("global::ValidIntegration.Register(services);");
        }
    }

    [Test]
    public async Task File_Local_Integration_Type_Reports_Diagnostic()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using Microsoft.Extensions.DependencyInjection;

            file static class FileLocalIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }
            }
            """);

        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPGEN001");
            await Assert.That(diagnostic.GetMessage()).Contains("FileLocalIntegration.Register");
            await Assert.That(result.GeneratedTrees).IsEmpty();
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

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            new ModularPipelinesIntegrationGenerator());

        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out _,
            out _);

        return driver.GetRunResult();
    }
}
