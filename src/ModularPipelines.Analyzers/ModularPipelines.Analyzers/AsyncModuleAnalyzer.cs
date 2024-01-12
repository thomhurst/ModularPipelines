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

    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AsyncModuleAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AsyncModuleAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AsyncModuleAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

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

        if (methodDeclarationSyntax.Identifier.Text != "ExecuteAsync")
        {
            return;
        }

        if (methodDeclarationSyntax.Modifiers.Any(x => x.ValueText == "async"))
        {
            return;
        }

        if (methodDeclarationSyntax.Modifiers.All(x => x.ValueText != "override"))
        {
            return;
        }

        if (context.GetClassThatNodeIsIn().GetSelfAndAllBaseTypes().All(x => x.Name != "ModuleBase"))
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

        if (methodSymbol.Name != "AsTask" && methodSymbol.Name != "FromResult")
        {
            return false;
        }

        if (methodSymbol.ContainingType.Name != "Task" && methodSymbol.ContainingType.Name != "TaskExtensions")
        {
            return false;
        }

        if (methodSymbol.ContainingNamespace.ToDisplayString() != "ModularPipelines.Extensions"
            && methodSymbol.ContainingNamespace.ToDisplayString() != "System.Threading.Tasks")
        {
            return false;
        }

        return true;
    }
}