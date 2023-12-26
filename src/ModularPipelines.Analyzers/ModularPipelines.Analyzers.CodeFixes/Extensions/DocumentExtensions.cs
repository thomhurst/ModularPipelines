using Microsoft.CodeAnalysis;

namespace ModularPipelines.Analyzers.Extensions;

internal static class DocumentExtensions
{
    public static async Task<Document> WithReplacedNode(
        this Document document,
        SyntaxNode oldNode,
        SyntaxNode newNode,
        SyntaxNode? root = null)
    {
        root ??= await document
            .GetSyntaxRootAsync()
            .ConfigureAwait(false);

        var newRoot = root!.ReplaceNode(oldNode, newNode);
        
        return document.WithSyntaxRoot(newRoot);
    }
}