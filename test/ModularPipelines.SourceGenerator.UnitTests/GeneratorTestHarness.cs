using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

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
        string source,
        string assemblyName = "GeneratorTests",
        IReadOnlyDictionary<string, string>? globalOptions = null)
    {
        var compilation = CreateCompilation(infrastructure, source, assemblyName);
        return Run(generator, compilation, globalOptions);
    }

    public static GeneratorDriverRunResult RunWithExternalAssembly(
        IIncrementalGenerator generator,
        string infrastructure,
        string externalSource,
        string source,
        IReadOnlyDictionary<string, string>? globalOptions = null)
    {
        var compilation = CreateCompilationWithExternalAssembly(infrastructure, externalSource, source);
        return Run(generator, compilation, globalOptions);
    }

    public static (GeneratorDriverRunResult Result, Diagnostic[] CompilationDiagnostics)
        RunWithExternalAssemblyAndGetCompilationDiagnostics(
            IIncrementalGenerator generator,
            string infrastructure,
            string externalSource,
            string source,
            IReadOnlyDictionary<string, string>? globalOptions = null)
    {
        var compilation = CreateCompilationWithExternalAssembly(infrastructure, externalSource, source);
        var driver = CreateDriver(generator, globalOptions).RunGeneratorsAndUpdateCompilation(
            compilation,
            out var outputCompilation,
            out _);

        return (driver.GetRunResult(), outputCompilation.GetDiagnostics().ToArray());
    }

    public static GeneratorDriverRunResult RunWithIndirectExternalAssembly(
        IIncrementalGenerator generator,
        string infrastructure,
        string externalBaseSource,
        string externalLeafSource,
        string source,
        IReadOnlyDictionary<string, string>? globalOptions = null)
    {
        var infrastructureReference = CreateMetadataReference(
            "ModularPipelines",
            [infrastructure],
            References);
        var externalBaseReference = CreateMetadataReference(
            "ExternalBase",
            [externalBaseSource],
            [.. References, infrastructureReference]);
        var externalLeafReference = CreateMetadataReference(
            "ExternalLeaf",
            [externalLeafSource],
            [.. References, infrastructureReference, externalBaseReference]);
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [CSharpSyntaxTree.ParseText(source)],
            [
                .. References,
                infrastructureReference,
                externalBaseReference,
                externalLeafReference,
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

        return Run(generator, compilation, globalOptions);
    }

    public static IReadOnlyList<string> GetIndirectExternalAssemblyClosure(
        string infrastructure,
        string externalBaseSource,
        string externalLeafSource)
    {
        var infrastructureReference = CreateMetadataReference(
            "ModularPipelines",
            [infrastructure],
            References);
        var externalBaseReference = CreateMetadataReference(
            "ExternalBase",
            [externalBaseSource],
            [.. References, infrastructureReference]);
        var externalLeafReference = CreateMetadataReference(
            "ExternalLeaf",
            [externalLeafSource],
            [.. References, infrastructureReference, externalBaseReference]);
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [CSharpSyntaxTree.ParseText("public sealed class Host;")],
            [
                .. References,
                infrastructureReference,
                externalBaseReference,
                externalLeafReference,
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

        var externalLeaf = compilation.SourceModule.ReferencedAssemblySymbols
            .Single(static assembly => assembly.Name == "ExternalLeaf");
        return CommandOptionsGenerator.GetReferencedAssemblyClosure([externalLeaf])
            .Select(static assembly => assembly.Name)
            .ToArray();
    }

    public static GeneratorDriverRunResult RunWithPeerExternalAssemblies(
        IIncrementalGenerator generator,
        string infrastructure,
        string firstExternalSource,
        string secondExternalSource,
        string source,
        IReadOnlyDictionary<string, string>? globalOptions = null,
        bool firstExternalReferencesInfrastructure = true)
    {
        var infrastructureReference = CreateMetadataReference(
            "ModularPipelines",
            [infrastructure],
            References);
        var firstExternalReference = CreateMetadataReference(
            "ExternalOne",
            [firstExternalSource],
            firstExternalReferencesInfrastructure
                ? [.. References, infrastructureReference]
                : References);
        var secondExternalReference = CreateMetadataReference(
            "ExternalTwo",
            [secondExternalSource],
            [.. References, infrastructureReference]);
        var compilation = CSharpCompilation.Create(
            "GeneratorTests",
            [CSharpSyntaxTree.ParseText(source)],
            [
                .. References,
                infrastructureReference,
                firstExternalReference,
                secondExternalReference,
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

        return Run(generator, compilation, globalOptions);
    }

    private static GeneratorDriverRunResult Run(
        IIncrementalGenerator generator,
        CSharpCompilation compilation,
        IReadOnlyDictionary<string, string>? globalOptions = null)
    {
        var driver = CreateDriver(generator, globalOptions).RunGeneratorsAndUpdateCompilation(
            compilation,
            out _,
            out _);

        return driver.GetRunResult();
    }

    private static GeneratorDriver CreateDriver(
        IIncrementalGenerator generator,
        IReadOnlyDictionary<string, string>? globalOptions) =>
        CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            optionsProvider: new TestAnalyzerConfigOptionsProvider(globalOptions));

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
        var sourceTree = compilation.SyntaxTrees.Last();
        var equivalentSourceTree = CSharpSyntaxTree.ParseText(
            sourceTree.GetText().ToString() + Environment.NewLine,
            (CSharpParseOptions) sourceTree.Options);
        var equivalentCompilation = compilation.ReplaceSyntaxTree(
            sourceTree,
            equivalentSourceTree);
        ThrowForCompilationErrors(equivalentCompilation);
        driver = driver.RunGenerators(equivalentCompilation);

        return driver.GetRunResult();
    }

    public static bool HasCachedOrUnchangedOutput(GeneratorDriverRunResult result)
    {
        var outputReasons = result.Results.Single().TrackedOutputSteps.Values
            .SelectMany(static steps => steps)
            .SelectMany(static step => step.Outputs)
            .Select(static output => output.Reason)
            .ToArray();

        return outputReasons.Length > 0
               && outputReasons.All(static reason => reason is
                   IncrementalStepRunReason.Cached
                   or IncrementalStepRunReason.Unchanged);
    }

    private static CSharpCompilation CreateCompilation(
        string infrastructure,
        string source,
        string assemblyName = "GeneratorTests")
    {
        var compilation = CSharpCompilation.Create(
            assemblyName,
            [
                CSharpSyntaxTree.ParseText(infrastructure),
                CSharpSyntaxTree.ParseText(source),
            ],
            References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ThrowForCompilationErrors(compilation);

        return compilation;
    }

    private static CSharpCompilation CreateCompilationWithExternalAssembly(
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

    internal sealed class TestAnalyzerConfigOptionsProvider(
        IReadOnlyDictionary<string, string>? globalOptions) : AnalyzerConfigOptionsProvider
    {
        private static readonly AnalyzerConfigOptions Empty =
            new DictionaryAnalyzerConfigOptions(new Dictionary<string, string>());

        public override AnalyzerConfigOptions GlobalOptions { get; } =
            new DictionaryAnalyzerConfigOptions(
                globalOptions ?? new Dictionary<string, string>());

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => Empty;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => Empty;
    }

    private sealed class DictionaryAnalyzerConfigOptions(
        IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, out string value) =>
            values.TryGetValue(key, out value!);
    }
}
