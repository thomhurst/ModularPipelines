using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class DuplicateDependsOnAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0019";

    public static DiagnosticDescriptor Rule { get; } =
        DiagnosticDescriptorFactory.Create(
            DiagnosticId,
            nameof(Resources.DuplicateDependsOnAnalyzerTitle),
            nameof(Resources.DuplicateDependsOnAnalyzerMessageFormat),
            nameof(Resources.DuplicateDependsOnAnalyzerDescription),
            severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        ModuleAuthoringAnalysis.InitializeDuplicateDependencyAnalysis(context);
    }
}
