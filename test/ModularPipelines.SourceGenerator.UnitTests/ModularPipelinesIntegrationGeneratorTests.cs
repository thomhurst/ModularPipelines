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
            await Assert.That(generatedSource)
                .Contains("\"ModularPipelines.ToolProperty:Git\", \"global::IGit\"");
            await Assert.That(generatedSource)
                .Contains("\"ModularPipelines.ToolTypeIdentity:Git\", \"GeneratorTests:IGit\"");
        }
    }

    [Test]
    public async Task Generic_Tool_Accessor_Generates_Runtime_Stable_Type_Identity()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IFoo<T>
            {
            }

            public static class FooIntegration
            {
                [ModularPipelinesIntegration]
                public static void AddFoo(IServiceCollection services)
                {
                }

                public static IFoo<string> Foo(this IPipelineContext context) => throw null!;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("\"ModularPipelines.ToolTypeIdentity:Foo\", " +
                          "\"GeneratorTests:IFoo`1[System.Private.CoreLib:System.String]\"");
        }
    }

    [Test]
    public async Task Value_Type_Accessor_Does_Not_Generate_Tool_Property()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public static class AvailabilityIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static bool IsAvailable(this IPipelineContext context) => true;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).DoesNotContain("IsAvailable");
            await Assert.That(generatedSource).DoesNotContain("tools.Get<bool>");
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
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0008");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Warning);
            await Assert.That(diagnostic.GetMessage()).Contains("C# 14");
            await Assert.That(diagnostic.GetMessage()).Contains("context.Git()");
            await Assert.That(generatedSource).DoesNotContain("extension(");
            await Assert.That(generatedSource)
                .Contains("global::GitIntegration.Register(services);");
            await Assert.That(generatedSource)
                .Contains("\"ModularPipelines.ToolProperty:Git\", \"global::IGit\"");
        }
    }

    [Test]
    public async Task Keyword_Tool_Accessor_On_Older_Language_Version_Is_Escaped_In_Diagnostic()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IClassTool
            {
            }

            public static class ClassIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IClassTool @class(this IPipelineContext context) => throw null!;
            }
            """, LanguageVersion.CSharp13);

        var diagnostic = result.Diagnostics.Single();

        await Assert.That(diagnostic.Id).IsEqualTo("MPG0008");
        await Assert.That(diagnostic.GetMessage()).Contains("context.@class()");
    }

    [Test]
    public async Task Referenced_Conflicts_On_Older_Language_Version_Are_Ignored()
    {
        var firstIntegration = CreateMetadataReference(
            "FirstIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::FirstIntegration.IGit")]
            """);
        var secondIntegration = CreateMetadataReference(
            "SecondIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::SecondIntegration.IGit")]
            """);

        var result = RunGenerator(
            """
            using ModularPipelines.Attributes;
            using Microsoft.Extensions.DependencyInjection;

            public static class ConsumerIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }
            }
            """,
            LanguageVersion.CSharp13,
            [firstIntegration, secondIntegration]);
        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("global::ConsumerIntegration.Register(services);");
            await Assert.That(generatedSource).DoesNotContain("extension(");
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
            .Where(static diagnostic => diagnostic.Id == "MPG0009")
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
    public async Task Conflicting_Referenced_Tool_Accessors_Report_Diagnostics()
    {
        var firstIntegration = CreateMetadataReference(
            "FirstIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::FirstIntegration.IGit")]
            """);
        var secondIntegration = CreateMetadataReference(
            "SecondIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::SecondIntegration.IGit")]
            """);

        var result = RunGenerator(
            source: string.Empty,
            additionalReferences: [firstIntegration, secondIntegration]);
        var diagnostics = result.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "MPG0009")
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostics).Count().IsEqualTo(2);
            await Assert.That(diagnostics.Select(static diagnostic => diagnostic.Severity))
                .IsEquivalentTo([DiagnosticSeverity.Error, DiagnosticSeverity.Error]);
            await Assert.That(diagnostics[0].GetMessage()).Contains("Git");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Identical_Referenced_Tool_Accessors_Report_Diagnostics()
    {
        var firstIntegration = CreateMetadataReference(
            "FirstIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::Shared.IGit")]
            """);
        var secondIntegration = CreateMetadataReference(
            "SecondIntegration",
            """
            [assembly: System.Reflection.AssemblyMetadata(
                "ModularPipelines.ToolProperty:Git",
                "global::Shared.IGit")]
            """);

        var result = RunGenerator(
            source: string.Empty,
            additionalReferences: [firstIntegration, secondIntegration]);
        var diagnostics = result.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "MPG0009")
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostics).Count().IsEqualTo(2);
            await Assert.That(diagnostics[0].GetMessage()).Contains("FirstIntegration");
            await Assert.That(diagnostics[0].GetMessage()).Contains("SecondIntegration");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Inaccessible_Tool_Return_Type_Does_Not_Generate_Property()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            internal interface IHiddenTool
            {
            }

            internal static class HiddenIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IHiddenTool Hidden(this IPipelineContext context) => throw null!;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource).DoesNotContain("ToolProperty:Hidden");
            await Assert.That(generatedSource).DoesNotContain("tools.Get<global::IHiddenTool>");
        }
    }

    [Test]
    public async Task Keyword_Tool_Accessor_Generates_Escaped_Property()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface IClassTool
            {
            }

            public static class ClassIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static IClassTool @class(this IPipelineContext context) => throw null!;
            }
            """);

        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(generatedSource)
                .Contains("public global::IClassTool @class => tools.Get<global::IClassTool>();");
            await Assert.That(generatedSource)
                .Contains("\"ModularPipelines.ToolProperty:class\", \"global::IClassTool\"");
        }
    }

    [Test]
    public async Task Instance_Member_Tool_Names_Report_Diagnostics()
    {
        var result = RunGenerator("""
            using ModularPipelines.Attributes;
            using ModularPipelines.Context;
            using Microsoft.Extensions.DependencyInjection;

            public interface ITool
            {
            }

            public static class ToolIntegration
            {
                [ModularPipelinesIntegration]
                public static void Register(IServiceCollection services)
                {
                }

                public static ITool Get(this IPipelineContext context) => throw null!;

                public static ITool GetType(this IPipelineContext context) => throw null!;
            }
            """);

        var diagnostics = result.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "MPG0010")
            .ToArray();
        var generatedSource = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostics).Count().IsEqualTo(2);
            await Assert.That(diagnostics.Select(static diagnostic => diagnostic.Severity))
                .IsEquivalentTo([DiagnosticSeverity.Error, DiagnosticSeverity.Error]);
            await Assert.That(diagnostics[0].GetMessage()).Contains("IToolsContext or object");
            await Assert.That(diagnostics[1].GetMessage()).Contains("IToolsContext or object");
            await Assert.That(generatedSource).DoesNotContain("public global::ITool Get");
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
    public async Task Equivalent_Compilation_Uses_Incremental_Cache()
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

        await Assert.That(GeneratorTestHarness.HasCachedOrUnchangedOutput(result)).IsTrue();
    }

    private static GeneratorDriverRunResult RunGenerator(
        string source,
        LanguageVersion languageVersion = LanguageVersion.Preview,
        IReadOnlyList<MetadataReference>? additionalReferences = null)
    {
        var parseOptions = new CSharpParseOptions(languageVersion);
        var infrastructureSyntaxTree = CSharpSyntaxTree.ParseText(
            TestInfrastructure,
            parseOptions);
        var sourceSyntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path))
            .Concat(additionalReferences ?? []);
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

    private static PortableExecutableReference CreateMetadataReference(
        string assemblyName,
        string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var stream = new MemoryStream();
        var emitResult = compilation.Emit(stream);
        if (!emitResult.Success)
        {
            throw new InvalidOperationException(string.Join(
                Environment.NewLine,
                emitResult.Diagnostics));
        }

        return MetadataReference.CreateFromImage(stream.ToArray());
    }
}
