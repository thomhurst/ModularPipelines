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
public class AwaitThisAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AwaitThis";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.AwaitThisAnalyzerTitle),
        nameof(Resources.AwaitThisAnalyzerMessageFormat),
        nameof(Resources.AwaitThisAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeAwaitExpression, SyntaxKind.AwaitExpression);
    }

    private void AnalyzeAwaitExpression(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AwaitExpressionSyntax awaitExpression)
        {
            return;
        }

        // Check if the awaited expression is 'this'
        if (awaitExpression.Expression is not ThisExpressionSyntax)
        {
            return;
        }

        // Check if we're inside a module class (inherits from ModuleBase)
        var containingClass = context.GetClassThatNodeIsIn();
        if (containingClass == null)
        {
            return;
        }

        if (containingClass.GetSelfAndAllBaseTypes().All(x => x.Name != AnalyzerConstants.TypeNames.ModuleBase))
        {
            return;
        }

        // Check if we're inside the OnAfterExecute method - if so, allow await this
        var containingMethod = awaitExpression.FirstAncestorOrSelf<MethodDeclarationSyntax>();
        if (containingMethod?.Identifier.Text == AnalyzerConstants.MethodNames.OnAfterExecute)
        {
            return;
        }

        // Report the diagnostic
        context.ReportDiagnostic(Diagnostic.Create(Rule, awaitExpression.GetLocation()));
    }
}