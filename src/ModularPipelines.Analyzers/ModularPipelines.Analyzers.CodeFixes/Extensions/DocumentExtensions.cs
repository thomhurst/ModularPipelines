using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace ModularPipelines.Analyzers.Extensions;

internal static class DocumentExtensions
{
    public static async Task<Document> GetDocumentWithReplacedNode(
        this CodeFixContext context,
        SyntaxNode oldNode,
        SyntaxNode newNode,
        SyntaxNode? root = null)
    {
        root ??= await context.Document
            .GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var newRoot = root!.ReplaceNode(oldNode, newNode);
        
        return context.Document.WithSyntaxRoot(newRoot);
    }
}