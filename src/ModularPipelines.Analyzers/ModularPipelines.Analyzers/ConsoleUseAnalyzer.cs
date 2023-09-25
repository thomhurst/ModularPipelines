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

    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ConsoleUseAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ConsoleUseAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ConsoleUseAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly DiagnosticDescriptor PrivateRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

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

        if (namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::System.Console")
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
