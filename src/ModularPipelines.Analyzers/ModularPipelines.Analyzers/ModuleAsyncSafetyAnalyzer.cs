using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class ModuleAsyncSafetyAnalyzer : DiagnosticAnalyzer
{
    public const string AsyncVoidId = "MPASYNC001";
    public const string BlockingCallId = "MPASYNC002";
    public const string UnflowedCancellationTokenId = "MPASYNC003";
    public const string ThreadSleepId = "MPASYNC004";

    public static DiagnosticDescriptor AsyncVoidRule { get; } =
        DiagnosticDescriptorFactory.Create(
            AsyncVoidId,
            nameof(Resources.AsyncVoidModuleAnalyzerTitle),
            nameof(Resources.AsyncVoidModuleAnalyzerMessageFormat),
            nameof(Resources.AsyncVoidModuleAnalyzerDescription));

    public static DiagnosticDescriptor BlockingCallRule { get; } =
        DiagnosticDescriptorFactory.Create(
            BlockingCallId,
            nameof(Resources.BlockingCallModuleAnalyzerTitle),
            nameof(Resources.BlockingCallModuleAnalyzerMessageFormat),
            nameof(Resources.BlockingCallModuleAnalyzerDescription),
            severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor UnflowedCancellationTokenRule { get; } =
        DiagnosticDescriptorFactory.Create(
            UnflowedCancellationTokenId,
            nameof(Resources.UnflowedCancellationTokenAnalyzerTitle),
            nameof(Resources.UnflowedCancellationTokenAnalyzerMessageFormat),
            nameof(Resources.UnflowedCancellationTokenAnalyzerDescription),
            severity: DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor ThreadSleepRule { get; } =
        DiagnosticDescriptorFactory.Create(
            ThreadSleepId,
            nameof(Resources.ThreadSleepModuleAnalyzerTitle),
            nameof(Resources.ThreadSleepModuleAnalyzerMessageFormat),
            nameof(Resources.ThreadSleepModuleAnalyzerDescription),
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
