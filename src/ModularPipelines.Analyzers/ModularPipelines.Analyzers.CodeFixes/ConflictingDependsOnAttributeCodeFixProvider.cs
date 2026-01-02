using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConflictingDependsOnAttributeCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class ConflictingDependsOnAttributeCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ConflictingDependsOnAttributeAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public sealed override FixAllProvider GetFixAllProvider()
    {
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

        // Find the attribute syntax identified by the diagnostic.
        var attributeSyntax = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();

        if (attributeSyntax is null)
        {
            return;
        }

        // Register a code action that will remove the conflicting attribute.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.ConflictingDependsOnAttributeCodeFixTitle,
                createChangedDocument: c => RemoveAttribute(context, attributeSyntax, c),
                equivalenceKey: nameof(CodeFixResources.ConflictingDependsOnAttributeCodeFixTitle)),
            diagnostic);
    }

    private static async Task<Document> RemoveAttribute(CodeFixContext context, AttributeSyntax attributeSyntax, CancellationToken cancellationToken)
    {
        var document = context.Document;
        var documentRoot = (await document.GetSyntaxRootAsync(cancellationToken))!;

        // Get the attribute list that contains this attribute
        var attributeList = attributeSyntax.Parent as AttributeListSyntax;
        if (attributeList is null)
        {
            return document;
        }

        SyntaxNode newRoot;

        // If this is the only attribute in the list, remove the entire attribute list
        if (attributeList.Attributes.Count == 1)
        {
            newRoot = documentRoot.RemoveNode(attributeList, SyntaxRemoveOptions.KeepNoTrivia)!;
        }
        else
        {
            // Otherwise, just remove this attribute from the list
            var newAttributes = attributeList.Attributes.Remove(attributeSyntax);
            var newAttributeList = attributeList.WithAttributes(newAttributes);
            newRoot = documentRoot.ReplaceNode(attributeList, newAttributeList);
        }

        return document.WithSyntaxRoot(newRoot);
    }
}
