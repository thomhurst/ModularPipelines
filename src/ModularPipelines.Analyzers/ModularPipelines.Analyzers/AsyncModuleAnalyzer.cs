using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class AsyncModuleAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AsyncModule";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.AsyncModuleAnalyzerTitle),
        nameof(Resources.AsyncModuleAnalyzerMessageFormat),
        nameof(Resources.AsyncModuleAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeNonAsyncModules, SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeNonAsyncModules(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return;
        }

        if (methodDeclarationSyntax.Identifier.Text != AnalyzerConstants.MethodNames.ExecuteAsync)
        {
            return;
        }

        if (methodDeclarationSyntax.Modifiers.Any(x => x.ValueText == AnalyzerConstants.Modifiers.Async))
        {
            return;
        }

        if (methodDeclarationSyntax.Modifiers.All(x => x.ValueText != AnalyzerConstants.Modifiers.Override))
        {
            return;
        }

        if (context.GetClassThatNodeIsIn().GetSelfAndAllBaseTypes().All(x => x.Name != AnalyzerConstants.TypeNames.ModuleBase))
        {
            return;
        }

        var returnStatementSyntaxes = methodDeclarationSyntax.Body?.DescendantNodes().OfType<ReturnStatementSyntax>().ToArray() ?? Array.Empty<ReturnStatementSyntax>();

        if (returnStatementSyntaxes.All(x => IsSynchronousObjectWrappedInTask(x, context)))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
    }

    private bool IsSynchronousObjectWrappedInTask(ReturnStatementSyntax returnStatementSyntax,
        SyntaxNodeAnalysisContext context)
    {
        if (returnStatementSyntax.ChildNodes().FirstOrDefault() is not InvocationExpressionSyntax
            invocationExpressionSyntax)
        {
            return false;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(invocationExpressionSyntax).Symbol;

        if (symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        if (methodSymbol.Name != AnalyzerConstants.MethodNames.AsTask && methodSymbol.Name != AnalyzerConstants.MethodNames.FromResult)
        {
            return false;
        }

        if (methodSymbol.ContainingType.Name != AnalyzerConstants.TypeNames.Task && methodSymbol.ContainingType.Name != AnalyzerConstants.TypeNames.TaskExtensions)
        {
            return false;
        }

        if (methodSymbol.ContainingNamespace.ToDisplayString() != AnalyzerConstants.Namespaces.ModularPipelinesExtensions
            && methodSymbol.ContainingNamespace.ToDisplayString() != AnalyzerConstants.Namespaces.SystemThreadingTasks)
        {
            return false;
        }

        return true;
    }
}