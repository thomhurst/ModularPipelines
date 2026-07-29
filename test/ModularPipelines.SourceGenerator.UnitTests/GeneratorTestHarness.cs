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
        return Run(generator, compilation);
    }

    public static GeneratorDriverRunResult RunWithExternalAssembly(
        IIncrementalGenerator generator,
        string infrastructure,
        string externalSource,
        string source)
    {
        var infrastructureReference = CreateMetadataReference(
            "ModularPipelines",
            [infrastructure],
            References);
        var externalReference = CreateMetadataReference(
            "ExternalModules",
            [externalSource],
            [.. References, infrastructureReference]);
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [CSharpSyntaxTree.ParseText(source)],
            [.. References, infrastructureReference, externalReference],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

        return Run(generator, compilation);
    }

    private static GeneratorDriverRunResult Run(
        IIncrementalGenerator generator,
        CSharpCompilation compilation)
    {
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
        ThrowForCompilationErrors(compilation);

        return compilation;
    }

    private static void ThrowForCompilationErrors(CSharpCompilation compilation)
    {
        var compilationErrors = compilation.GetDiagnostics()
            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        if (compilationErrors.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, compilationErrors));
        }
    }

    private static PortableExecutableReference CreateMetadataReference(
        string assemblyName,
        IEnumerable<string> sources,
        IEnumerable<MetadataReference> references)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName,
            sources.Select(static source => CSharpSyntaxTree.ParseText(source)),
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

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
