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
}
