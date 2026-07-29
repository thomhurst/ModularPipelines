using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class DuplicateDependsOnAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MPDEP004";

    public static DiagnosticDescriptor Rule { get; } =
        DiagnosticDescriptorFactory.Create(
            DiagnosticId,
            "DuplicateDependsOnAnalyzerTitle",
            "DuplicateDependsOnAnalyzerMessageFormat",
            "DuplicateDependsOnAnalyzerDescription");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        ModuleAuthoringAnalysis.InitializeDuplicateDependencyAnalysis(context);
    }
}
