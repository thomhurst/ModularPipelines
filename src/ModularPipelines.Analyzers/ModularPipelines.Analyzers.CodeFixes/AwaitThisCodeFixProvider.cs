using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AwaitThisCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class AwaitThisCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [AwaitThisAnalyzer.DiagnosticId];

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var awaitExpression = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<AwaitExpressionSyntax>();

        if (awaitExpression?.Parent is not ExpressionStatementSyntax expressionStatement
            || expressionStatement.ContainsDirectives
            || IsInsideLoop(expressionStatement)
            || IsGotoTarget(expressionStatement))
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.AwaitThisCodeFixTitle,
                cancellationToken => RemoveAwaitAsync(context.Document, expressionStatement, cancellationToken),
                nameof(CodeFixResources.AwaitThisCodeFixTitle)),
            diagnostic);
    }

    private static bool IsInsideLoop(ExpressionStatementSyntax statement)
    {
        return statement.Ancestors().Any(ancestor =>
            ancestor is WhileStatementSyntax
                or DoStatementSyntax
                or ForStatementSyntax
                or ForEachStatementSyntax
                or ForEachVariableStatementSyntax);
    }

    private static bool IsGotoTarget(ExpressionStatementSyntax statement)
    {
        var labeledStatement = statement
            .AncestorsAndSelf()
            .OfType<LabeledStatementSyntax>()
            .FirstOrDefault();
        if (labeledStatement is null)
        {
            return false;
        }

        var callable = statement.Ancestors().FirstOrDefault(static ancestor =>
            ancestor is BaseMethodDeclarationSyntax
                or AccessorDeclarationSyntax
                or LocalFunctionStatementSyntax
                or AnonymousFunctionExpressionSyntax);
        var scope = callable ?? statement.SyntaxTree.GetRoot();

        return scope.DescendantNodes()
            .OfType<GotoStatementSyntax>()
            .Any(gotoStatement =>
                gotoStatement.Expression is IdentifierNameSyntax identifier
                && identifier.Identifier.ValueText == labeledStatement.Identifier.ValueText);
    }

    private static async Task<Document> RemoveAwaitAsync(
        Document document,
        ExpressionStatementSyntax expressionStatement,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        if (expressionStatement.Parent is not (BlockSyntax or SwitchSectionSyntax))
        {
            return document.WithSyntaxRoot(root.ReplaceNode(
                expressionStatement,
                SyntaxFactory.EmptyStatement()
                    .WithTriviaFrom(expressionStatement)
                    .WithAdditionalAnnotations(Formatter.Annotation)));
        }

        var newRoot = root.RemoveNode(
            expressionStatement,
            SyntaxRemoveOptions.KeepExteriorTrivia);
        if (newRoot is null)
        {
            return document;
        }

        var nextNode = newRoot.FindToken(expressionStatement.SpanStart).Parent;
        if (nextNode is not null)
        {
            newRoot = newRoot.ReplaceNode(
                nextNode,
                nextNode.WithAdditionalAnnotations(Formatter.Annotation));
        }

        return document.WithSyntaxRoot(newRoot);
    }
}
