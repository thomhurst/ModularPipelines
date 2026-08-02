using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AsyncModuleCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class AsyncModuleCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsyncModuleAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    /// <inheritdoc/>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the type declaration identified by the diagnostic.
        var declaration = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

        if (declaration is null)
        {
            return;
        }

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.AsyncModuleCodeFixTitle,
                createChangedDocument: c => AddAsync(context, declaration, c),
                equivalenceKey: nameof(CodeFixResources.AsyncModuleCodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> AddAsync(CodeFixContext context, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(context.Document, cancellationToken);

        editor.SetModifiers(methodDeclarationSyntax, DeclarationModifiers.Override | DeclarationModifiers.Async);

        // Cache the semantic model to avoid duplicate async calls
        var semanticModel = await context.Document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        if (methodDeclarationSyntax.ExpressionBody is { Expression: var expression })
        {
            editor.ReplaceNode(
                expression,
                RewriteReturnExpression(expression, semanticModel));
        }

        foreach (var returnStatement in GetReturnStatements(methodDeclarationSyntax))
        {
            var expressionSyntax = returnStatement.Expression;

            // Skip return statements without expressions (e.g., bare "return;")
            if (expressionSyntax is null)
            {
                continue;
            }

            editor.ReplaceNode(
                expressionSyntax,
                RewriteReturnExpression(expressionSyntax, semanticModel));
        }

        return editor.GetChangedDocument();
    }

    private static ReturnStatementSyntax[] GetReturnStatements(MethodDeclarationSyntax newMethodDeclarationSyntax)
    {
        return newMethodDeclarationSyntax.Body
                   ?.DescendantNodes(ShouldDescendInto)
                   .OfType<ReturnStatementSyntax>()
                   .Reverse()
                   .ToArray()
               ?? Array.Empty<ReturnStatementSyntax>();
    }

    private static bool ShouldDescendInto(SyntaxNode node) =>
        node is not AnonymousFunctionExpressionSyntax
            and not LocalFunctionStatementSyntax;

    private static ExpressionSyntax RewriteReturnExpression(
        ExpressionSyntax expression,
        SemanticModel? semanticModel)
    {
        if (expression is InvocationExpressionSyntax invocation
            && semanticModel?.GetSymbolInfo(invocation).Symbol is IMethodSymbol
            {
                Name: AnalyzerConstants.MethodNames.FromResult,
                ContainingType.Name: AnalyzerConstants.TypeNames.Task,
            } method
            && method.ContainingNamespace.ToDisplayString()
            == AnalyzerConstants.Namespaces.SystemThreadingTasks
            && invocation.ArgumentList.Arguments.FirstOrDefault()?.Expression is { } innerExpression)
        {
            return innerExpression;
        }

        var awaitOperand = expression.WithoutTrivia();
        if (awaitOperand is ConditionalExpressionSyntax or SwitchExpressionSyntax)
        {
            awaitOperand = SyntaxFactory.ParenthesizedExpression(awaitOperand);
        }

        return SyntaxFactory
            .AwaitExpression(awaitOperand)
            .WithTriviaFrom(expression);
    }
}
