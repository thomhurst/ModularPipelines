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
    public const string DiagnosticId = "MP0006";

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

        if (!context.GetClassThatNodeIsIn().IsModule(context.Compilation))
        {
            return;
        }

        var returnExpressions = GetReturnExpressions(methodDeclarationSyntax);

        if (returnExpressions.All(expression =>
                IsSynchronousObjectWrappedInTask(expression, context)))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
    }

    private static ExpressionSyntax[] GetReturnExpressions(
        MethodDeclarationSyntax methodDeclaration)
    {
        if (methodDeclaration.ExpressionBody is { Expression: var expression })
        {
            return [expression];
        }

        return methodDeclaration.Body is null
            ? []
            :
            [
                .. methodDeclaration.Body
                    .DescendantNodes(ShouldDescendInto)
                    .OfType<ReturnStatementSyntax>()
                    .Select(static statement => statement.Expression)
                    .OfType<ExpressionSyntax>(),
            ];
    }

    private static bool ShouldDescendInto(SyntaxNode node) =>
        node is not AnonymousFunctionExpressionSyntax
            and not LocalFunctionStatementSyntax;

    private static bool IsSynchronousObjectWrappedInTask(
        ExpressionSyntax expression,
        SyntaxNodeAnalysisContext context)
    {
        if (expression is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return false;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(invocationExpressionSyntax).Symbol;

        if (symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        if (methodSymbol.Name != AnalyzerConstants.MethodNames.FromResult)
        {
            return false;
        }

        if (methodSymbol.ContainingType.Name != AnalyzerConstants.TypeNames.Task)
        {
            return false;
        }

        return methodSymbol.ContainingNamespace.ToDisplayString()
               == AnalyzerConstants.Namespaces.SystemThreadingTasks;
    }
}
