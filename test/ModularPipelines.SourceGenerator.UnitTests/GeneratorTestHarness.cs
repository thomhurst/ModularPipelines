using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ModularPipelines.SourceGenerator.UnitTests;

internal static class GeneratorTestHarness
{
    private static readonly MetadataReference[] References =
    [
        ..
        ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
        .Split(Path.PathSeparator)
        .Select(static path => MetadataReference.CreateFromFile(path)),
    ];

    public static GeneratorDriverRunResult Run(
        IIncrementalGenerator generator,
        string infrastructure,
        string source)
    {
        var compilation = CreateCompilation(infrastructure, source);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out _,
            out _);

        return driver.GetRunResult();
    }

    public static GeneratorDriverRunResult RunTwiceWithStepTracking(
        IIncrementalGenerator generator,
        string infrastructure,
        string source)
    {
        var compilation = CreateCompilation(infrastructure, source);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new GeneratorDriverOptions(
                IncrementalGeneratorOutputKind.None,
                trackIncrementalGeneratorSteps: true));

        driver = driver.RunGenerators(compilation);
        driver = driver.RunGenerators(compilation);

        return driver.GetRunResult();
    }

    public static bool HasCachedOutput(GeneratorDriverRunResult result)
    {
        var outputReasons = result.Results.Single().TrackedOutputSteps.Values
            .SelectMany(static steps => steps)
            .SelectMany(static step => step.Outputs)
            .Select(static output => output.Reason)
            .ToArray();

        return outputReasons.Length > 0
               && outputReasons.All(static reason => reason == IncrementalStepRunReason.Cached);
    }

    private static CSharpCompilation CreateCompilation(string infrastructure, string source)
    {
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [
                CSharpSyntaxTree.ParseText(infrastructure),
                CSharpSyntaxTree.ParseText(source),
            ],
            References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var compilationErrors = compilation.GetDiagnostics()
            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();

        return compilationErrors.Length == 0
            ? compilation
            : throw new InvalidOperationException(string.Join(Environment.NewLine, compilationErrors));
    }
}
