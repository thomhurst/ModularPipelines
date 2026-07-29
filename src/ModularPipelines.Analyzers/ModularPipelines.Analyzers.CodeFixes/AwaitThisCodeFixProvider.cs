using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AwaitThisCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class AwaitThisCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(AwaitThisAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var awaitExpression = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<AwaitExpressionSyntax>();

        if (awaitExpression?.Parent is not ExpressionStatementSyntax expressionStatement)
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

    private static async Task<Document> RemoveAwaitAsync(
        Document document,
        ExpressionStatementSyntax expressionStatement,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root?.RemoveNode(expressionStatement, SyntaxRemoveOptions.KeepNoTrivia);
        return newRoot is null ? document : document.WithSyntaxRoot(newRoot);
    }
}
