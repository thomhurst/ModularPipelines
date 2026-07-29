using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class ModuleAsyncSafetyAnalyzer : DiagnosticAnalyzer
{
    public const string AsyncVoidId = "MP0014";
    public const string BlockingCallId = "MP0015";
    public const string UnflowedCancellationTokenId = "MP0016";
    public const string ThreadSleepId = "MP0017";

    public static DiagnosticDescriptor AsyncVoidRule { get; } =
        DiagnosticDescriptorFactory.Create(
            AsyncVoidId,
            "AsyncVoidModuleAnalyzerTitle",
            "AsyncVoidModuleAnalyzerMessageFormat",
            "AsyncVoidModuleAnalyzerDescription");

    public static DiagnosticDescriptor BlockingCallRule { get; } =
        DiagnosticDescriptorFactory.Create(
            BlockingCallId,
            "BlockingCallModuleAnalyzerTitle",
            "BlockingCallModuleAnalyzerMessageFormat",
            "BlockingCallModuleAnalyzerDescription",
            severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor UnflowedCancellationTokenRule { get; } =
        DiagnosticDescriptorFactory.Create(
            UnflowedCancellationTokenId,
            "UnflowedCancellationTokenAnalyzerTitle",
            "UnflowedCancellationTokenAnalyzerMessageFormat",
            "UnflowedCancellationTokenAnalyzerDescription",
            severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor ThreadSleepRule { get; } =
        DiagnosticDescriptorFactory.Create(
            ThreadSleepId,
            "ThreadSleepModuleAnalyzerTitle",
            "ThreadSleepModuleAnalyzerMessageFormat",
            "ThreadSleepModuleAnalyzerDescription",
            severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        AsyncVoidRule,
        BlockingCallRule,
        UnflowedCancellationTokenRule,
        ThreadSleepRule,
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        ModuleAuthoringAnalysis.InitializeAsyncSafetyAnalysis(context);
    }
}
