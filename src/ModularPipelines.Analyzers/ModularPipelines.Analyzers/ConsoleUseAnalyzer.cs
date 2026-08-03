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

        var consoleType = context.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.SystemConsole);
        var symbolInfo = context.SemanticModel.GetSymbolInfo(
            invocationExpressionSyntax,
            context.CancellationToken);
        if (consoleType is not null
            && IsConsoleUse(
                context,
                invocationExpressionSyntax,
                symbolInfo,
                consoleType))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(),
                consoleType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static bool IsConsoleUse(
        SyntaxNodeAnalysisContext context,
        InvocationExpressionSyntax invocation,
        SymbolInfo symbolInfo,
        INamedTypeSymbol consoleType)
    {
        var resolvedMethodIsConsole = symbolInfo.Symbol is IMethodSymbol method
            && SymbolEqualityComparer.Default.Equals(method.ContainingType, consoleType);
        var candidateMethodIsConsole = symbolInfo.CandidateSymbols
            .OfType<IMethodSymbol>()
            .Any(candidate => IsConsoleMethod(candidate, consoleType));
        if (resolvedMethodIsConsole || candidateMethodIsConsole)
        {
            return true;
        }

        var memberGroupMethodIsConsole = context.SemanticModel
            .GetMemberGroup(invocation.Expression, context.CancellationToken)
            .OfType<IMethodSymbol>()
            .Any(candidate => IsConsoleMethod(candidate, consoleType));
        if (memberGroupMethodIsConsole)
        {
            return true;
        }

        var receiver = (invocation.Expression as MemberAccessExpressionSyntax)?.Expression;
        while (receiver is not null)
        {
            if (receiver is InvocationExpressionSyntax)
            {
                return false;
            }

            var receiverSymbol = context.SemanticModel.GetSymbolInfo(
                receiver,
                context.CancellationToken).Symbol;
            if (SymbolEqualityComparer.Default.Equals(receiverSymbol as INamedTypeSymbol, consoleType)
                || SymbolEqualityComparer.Default.Equals(receiverSymbol?.ContainingType, consoleType))
            {
                return true;
            }

            receiver = receiver switch
            {
                MemberAccessExpressionSyntax memberAccess => memberAccess.Expression,
                ElementAccessExpressionSyntax elementAccess => elementAccess.Expression,
                _ => null,
            };
        }

        return false;
    }

    private static bool IsConsoleMethod(IMethodSymbol method, INamedTypeSymbol consoleType) =>
        SymbolEqualityComparer.Default.Equals(method.ContainingType, consoleType);
}
