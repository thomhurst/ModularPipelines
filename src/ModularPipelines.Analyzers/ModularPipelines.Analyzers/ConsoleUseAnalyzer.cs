using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class ConsoleUseAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ConsoleUse";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.ConsoleUseAnalyzerTitle),
        nameof(Resources.ConsoleUseAnalyzerMessageFormat),
        nameof(Resources.ConsoleUseAnalyzerDescription));

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeConsoleUse, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeConsoleUse(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return;
        }

        if (invocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            return;
        }

        memberAccessExpressionSyntax = GetTopMemberAccessExpression(memberAccessExpressionSyntax);

        if (memberAccessExpressionSyntax.Expression is not IdentifierNameSyntax identifierNameSyntax)
        {
            return;
        }

        var nameSymbol = context.SemanticModel.GetSymbolInfo(identifierNameSyntax);

        if (nameSymbol.Symbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return;
        }

        if (namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == AnalyzerConstants.FullyQualifiedTypeNames.SystemConsole)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(),
                namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static MemberAccessExpressionSyntax GetTopMemberAccessExpression(MemberAccessExpressionSyntax memberAccessExpressionSyntax)
    {
        var memberAccessExpression = memberAccessExpressionSyntax;

        while (memberAccessExpression.Expression is MemberAccessExpressionSyntax parentMemberAccessExpressionSyntax)
        {
            memberAccessExpression = parentMemberAccessExpressionSyntax;
        }

        return memberAccessExpression;
    }
}