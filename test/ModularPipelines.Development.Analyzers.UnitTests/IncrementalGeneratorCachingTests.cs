using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using ModularPipelines.SourceGenerator;

namespace ModularPipelines.Development.Analyzers.UnitTests;

public class IncrementalGeneratorCachingTests
{
    [Test]
    public async Task Module_Extensions_Are_Cached_After_Trivia_Changes()
    {
        const string source = """
                              namespace ModularPipelines.Modules
                              {
                                  public abstract class Module<T>;
                              }

                              namespace Consumer
                              {
                                  public class CacheTarget : ModularPipelines.Modules.Module<string>;
                              }
                              """;

        await AssertOutputIsCachedAfterTriviaChange(new ModuleExtensionsGenerator(), source);
    }

    [Test]
    public async Task Command_Metadata_Is_Cached_After_Trivia_Changes()
    {
        const string source = """
                              namespace ModularPipelines.Options
                              {
                                  public abstract class CommandLineToolOptions;
                              }

                              namespace ModularPipelines.Attributes
                              {
                                  [System.AttributeUsage(System.AttributeTargets.Property)]
                                  public sealed class CliOptionAttribute(string name) : System.Attribute;
                              }

                              namespace Consumer
                              {
                                  public class CacheTarget : ModularPipelines.Options.CommandLineToolOptions
                                  {
                                      [ModularPipelines.Attributes.CliOptionAttribute("--value")]
                                      public string? Value { get; init; }
                                  }
                              }
                              """;

        await AssertOutputIsCachedAfterTriviaChange(new CommandOptionsGenerator(), source);
    }

    [Test]
    public async Task Module_Event_Metadata_Is_Cached_After_Trivia_Changes()
    {
        const string source = """
                              namespace ModularPipelines.Modules
                              {
                                  public abstract class Module<T>;
                              }

                              namespace Consumer
                              {
                                  [System.AttributeUsage(System.AttributeTargets.Class)]
                                  public sealed class SampleAttribute(string value) : System.Attribute;

                                  [Sample("value")]
                                  public class CacheTarget : ModularPipelines.Modules.Module<string>;
                              }
                              """;

        await AssertOutputIsCachedAfterTriviaChange(new ModuleEventMetadataGenerator(), source);
    }

    private static async Task AssertOutputIsCachedAfterTriviaChange(
        IIncrementalGenerator generator,
        string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create(
            "GeneratorCachingTests",
            [syntaxTree],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [generator.AsSourceGenerator()],
            driverOptions: new GeneratorDriverOptions(
                IncrementalGeneratorOutputKind.None,
                trackIncrementalGeneratorSteps: true));

        driver = driver.RunGenerators(compilation);

        var changedSource = source.Replace(
            "public class CacheTarget",
            "public  class CacheTarget",
            StringComparison.Ordinal);
        var changedSyntaxTree = syntaxTree.WithChangedText(SourceText.From(changedSource, Encoding.UTF8));
        driver = driver.RunGenerators(compilation.ReplaceSyntaxTree(syntaxTree, changedSyntaxTree));

        var result = driver.GetRunResult().Results.Single();
        var outputReasons = result.TrackedOutputSteps.Values
            .SelectMany(steps => steps)
            .SelectMany(step => step.Outputs)
            .Select(output => output.Reason)
            .ToArray();

        await Assert.That(result.GeneratedSources).IsNotEmpty();
        await Assert.That(outputReasons).IsNotEmpty();
        await Assert.That(outputReasons.All(reason => reason == IncrementalStepRunReason.Cached)).IsTrue();
    }
}
