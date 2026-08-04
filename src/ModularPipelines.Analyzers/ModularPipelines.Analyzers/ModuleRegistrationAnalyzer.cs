using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class ModuleRegistrationAnalyzer : DiagnosticAnalyzer
{
    public const string UnregisteredModuleId = "MP0013";
    public const string NonPublicModuleId = "MP0018";

    public static DiagnosticDescriptor UnregisteredModuleRule { get; } =
        DiagnosticDescriptorFactory.Create(
            UnregisteredModuleId,
            nameof(Resources.UnregisteredModuleAnalyzerTitle),
            nameof(Resources.UnregisteredModuleAnalyzerMessageFormat),
            nameof(Resources.UnregisteredModuleAnalyzerDescription),
            severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor NonPublicModuleRule { get; } =
        DiagnosticDescriptorFactory.Create(
            NonPublicModuleId,
            nameof(Resources.NonPublicModuleAnalyzerTitle),
            nameof(Resources.NonPublicModuleAnalyzerMessageFormat),
            nameof(Resources.NonPublicModuleAnalyzerDescription),
            severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        UnregisteredModuleRule,
        NonPublicModuleRule,
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        ModuleAuthoringAnalysis.InitializeRegistrationAnalysis(context);
    }
}
