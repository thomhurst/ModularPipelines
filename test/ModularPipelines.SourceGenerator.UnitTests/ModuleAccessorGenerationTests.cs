using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModuleAccessorGenerationTests
{
    private const string Infrastructure = """
        namespace ModularPipelines.Modules
        {
            public interface IModule;
        }

        namespace ModularPipelines
        {
            public abstract class Module<T> : Modules.IModule;
        }
        """;

    [Test]
    public async Task Modules_Do_Not_Generate_Convenience_Accessors()
    {
        foreach (var generator in CreateGenerators())
        {
            var result = GeneratorTestHarness.Run(generator, Infrastructure, """
                namespace Consumer
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                    internal sealed class InternalModule : ModularPipelines.Module<string>;
                    public sealed class GenericModule<T> : ModularPipelines.Module<T>;
                    public class Container
                    {
                        public sealed class NestedModule : ModularPipelines.Module<string>;
                    }
                }
                """);

            await AssertNoConvenienceAccessors(result);
            if (generator is ModuleMetadataGenerator)
            {
                await Assert.That(result.GeneratedTrees.Single().GetText().ToString())
                    .Contains("CreateRegistration<global::Consumer.BuildModule, string>");
            }
        }
    }

    [Test]
    public async Task Same_Named_Local_Modules_Do_Not_Conflict()
    {
        foreach (var generator in CreateGenerators())
        {
            var result = GeneratorTestHarness.Run(generator, Infrastructure, """
                namespace First
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }

                namespace Second
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }
                """);

            await AssertNoConvenienceAccessors(result);
            await Assert.That(result.Diagnostics.Any(diagnostic => diagnostic.Id == "MPG0002")).IsFalse();
        }
    }

    [Test]
    public async Task Same_Named_Modules_In_Referenced_Libraries_Do_Not_Generate_Accessors()
    {
        foreach (var generator in CreateGenerators())
        {
            var result = GeneratorTestHarness.RunWithPeerExternalAssemblies(
                generator,
                Infrastructure,
                """
                namespace First
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }
                """,
                """
                namespace Second
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }
                """,
                """
                namespace Consumer
                {
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }
                """);

            await AssertNoConvenienceAccessors(result);
            await Assert.That(result.Diagnostics.Any(diagnostic => diagnostic.Id == "MPG0002")).IsFalse();
        }
    }

    private static IEnumerable<IIncrementalGenerator> CreateGenerators() =>
        typeof(ModuleMetadataGenerator).Assembly.GetTypes()
            .Where(type => type.IsDefined(typeof(GeneratorAttribute), inherit: false))
            .Select(type => (IIncrementalGenerator) Activator.CreateInstance(type)!);

    private static async Task AssertNoConvenienceAccessors(GeneratorDriverRunResult result)
    {
        await Assert.That(result.Results.All(generator => generator.Exception is null)).IsTrue();
        var declarations = result.GeneratedTrees
            .SelectMany(tree => tree.GetRoot().DescendantNodes())
            .ToArray();

        await Assert.That(declarations.OfType<ClassDeclarationSyntax>().Any(type =>
            type.Identifier.ValueText.EndsWith("ModuleContextExtensions", StringComparison.Ordinal))).IsFalse();
        await Assert.That(declarations.OfType<MethodDeclarationSyntax>().Any(method =>
            method.Identifier.ValueText.StartsWith("Get", StringComparison.Ordinal)
            && (method.Identifier.ValueText.EndsWith("Module", StringComparison.Ordinal)
                || method.Identifier.ValueText.EndsWith("ModuleIfRegistered", StringComparison.Ordinal)))).IsFalse();
    }
}
