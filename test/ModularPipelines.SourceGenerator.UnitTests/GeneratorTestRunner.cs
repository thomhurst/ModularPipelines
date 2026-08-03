using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ModularPipelines.SourceGenerator.UnitTests;

internal static class GeneratorTestRunner
{
    public static GeneratorDriverRunResult Run(
        IIncrementalGenerator generator,
        params string[] sources)
    {
        var compilation = CreateCompilation(sources);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out _);
        return driver.GetRunResult();
    }

    public static GeneratorDriverRunResult RunIncrementalUpdate(
        IIncrementalGenerator generator,
        string[] initialSources,
        string[] updatedSources)
    {
        var initialCompilation = CreateCompilation(initialSources);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(initialCompilation, out _, out _);

        var updatedCompilation = CreateCompilation(updatedSources);
        driver = driver.RunGeneratorsAndUpdateCompilation(updatedCompilation, out _, out _);
        return driver.GetRunResult();
    }

    private static CSharpCompilation CreateCompilation(string[] sources)
    {
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            sources.Select(static (source, index) =>
                CSharpSyntaxTree.ParseText(source, path: $"Source{index}.cs")),
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var compilationErrors = compilation.GetDiagnostics()
            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        if (compilationErrors.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, compilationErrors));
        }

        return compilation;
    }
}
