using Microsoft.CodeAnalysis;

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

        namespace ModularPipelines.Context
        {
            public interface IPipelineContext
            {
            }

            public interface IToolsContext
            {
                T Get<T>() where T : class;
            }
        }
        """;

    [Test]
    public async Task Invalid_Integration_Method_Reports_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModularPipelinesIntegrationGenerator(), TestInfrastructure, """
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
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0001");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage()).Contains("InvalidIntegration.Register");
            await Assert.That(diagnostic.Descriptor.HelpLinkUri).EndsWith("#mpg0001");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Valid_Integration_Method_Generates_Registrar()
    {
        var result = GeneratorTestHarness.Run(new ModularPipelinesIntegrationGenerator(), TestInfrastructure, """
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

        await Assert.That(result.Diagnostics).IsEmpty();
        await SnapshotVerifier.VerifyAsync(
            "ModularPipelinesIntegrationGenerator.ValidIntegration",
            result.GeneratedTrees.Single().GetText().ToString());
    }

    [Test]
    public async Task Tool_Accessor_Generates_Discoverable_Extension_Property()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IGit
            {
            }

            public static class GitIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IGit Git(this IPipelineContext context) => throw null!;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("public global::IGit Git => tools.Get<global::IGit>();");
        }
    }

    [Test]
    public async Task Tool_Accessor_On_Older_Language_Version_Reports_Diagnostic()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IGit
            {
            }

            public static class GitIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IGit Git(this IPipelineContext context) => throw null!;
            }
            """, LanguageVersion.CSharp13);

        var diagnostic = result.Diagnostics.Single();
        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPGEN003");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Warning);
            await Assert.That(diagnostic.GetMessage()).Contains("C# 14");
            await Assert.That(generatedSource).DoesNotContain("extension(");
            await Assert.That(generatedSource)
                .Contains("global::GitIntegration.Register(services);");
        }
    }

    [Test]
    public async Task Conflicting_Tool_Accessors_Report_Diagnostics()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IGit
            {
            }

            public interface IAlternateGit
            {
            }

            public static class GitIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IGit Git(this IPipelineContext context) => throw null!;
            }

            public static class AlternateGitIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IAlternateGit Git(this IPipelineContext context) => throw null!;
            }
            """);

        var diagnostics = result.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "MPGEN004")
            .ToArray();
        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostics).Count().IsEqualTo(2);
            await Assert.That(diagnostics.Select(static diagnostic => diagnostic.Severity))
                .IsEquivalentTo([DiagnosticSeverity.Error, DiagnosticSeverity.Error]);
            await Assert.That(diagnostics[0].GetMessage()).Contains("Git");
            await Assert.That(generatedSource).DoesNotContain("tools.Get");
        }
    }

    [Test]
    public async Task File_Local_Integration_Type_Reports_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModularPipelinesIntegrationGenerator(), TestInfrastructure, """
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
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0001");
            await Assert.That(diagnostic.GetMessage()).Contains("FileLocalIntegration.Register");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task By_Reference_Parameter_Reports_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModularPipelinesIntegrationGenerator(), TestInfrastructure, """
            using ModularPipelines.Attributes;
            using Microsoft.Extensions.DependencyInjection;

            public static class ByReferenceIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(ref IServiceCollection services)
                {
                }
            }
            """);

        var diagnostic = result.Diagnostics.Single();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0001");
            await Assert.That(diagnostic.GetMessage()).Contains("ByReferenceIntegration.Register");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Unchanged_Compilation_Uses_Incremental_Cache()
    {
        var result = GeneratorTestHarness.RunTwiceWithStepTracking(
            new ModularPipelinesIntegrationGenerator(),
            TestInfrastructure,
            """
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

        await Assert.That(GeneratorTestHarness.HasCachedOutput(result)).IsTrue();
    }

    private static GeneratorDriverRunResult RunGenerator(
        string source,
        LanguageVersion languageVersion = LanguageVersion.Preview)
    {
        var parseOptions = new CSharpParseOptions(languageVersion);
        var infrastructureSyntaxTree = CSharpSyntaxTree.ParseText(
            TestInfrastructure,
            parseOptions);
        var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);
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
            [new ModularPipelinesIntegrationGenerator().AsSourceGenerator()],
            parseOptions: parseOptions);

        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out _,
            out _);

        return driver.GetRunResult();
    }
}
