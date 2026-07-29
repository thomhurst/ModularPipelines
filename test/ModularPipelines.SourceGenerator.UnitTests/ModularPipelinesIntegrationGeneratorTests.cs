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
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0001");
            await Assert.That(diagnostic.GetMessage()).Contains("FileLocalIntegration.Register");
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task By_Reference_Parameter_Reports_Diagnostic()
    {
        var result = RunGenerator("""
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

    private static GeneratorDriverRunResult RunGenerator(string source)
    {
        return GeneratorTestRunner.Run(
            new ModularPipelinesIntegrationGenerator(),
            TestInfrastructure,
            source);
    }
}
