using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace ModularPipelines.Development.Analyzers;

/// <summary>
/// A sample code fix provider that renames classes with the company name in their definition.
/// All code fixes must  be linked to specific analyzers.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(VirtualCommandCodeFixProvider)), Shared]
public class VirtualCommandCodeFixProvider : CodeFixProvider
{
    private const string CommonName = "Common";

    // Specify the diagnostic IDs of analyzers that are expected to be linked.
    public override sealed ImmutableArray<string> FixableDiagnosticIds { get; } = [VirtualCommandAnalyzer.DiagnosticId];

    // If you don't need the 'fix all' behaviour, return null.
    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override sealed async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // We link only one diagnostic and assume there is only one diagnostic in the context.
        var diagnostic = context.Diagnostics.Single();

        // 'SourceSpan' of 'Location' is the highlighted area. We're going to use this area to find the 'SyntaxNode' to rename.
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Get the root of Syntax Tree that contains the highlighted diagnostic.
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        // Find SyntaxNode corresponding to the diagnostic.
        var diagnosticNode = root?.FindNode(diagnosticSpan);

        // To get the required metadata, we should match the Node to the specific type: 'ClassDeclarationSyntax'.
        if (diagnosticNode is not MethodDeclarationSyntax declaration)
        {
            return;
        }

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: Resources.MPD0002CodeFixTitle,
                createChangedDocument: c => AddVirtualKeyword(context.Document, declaration, c),
                equivalenceKey: nameof(Resources.MPD0002CodeFixTitle)),
            diagnostic);
    }

    /// <summary>
    /// Executed on the quick fix action raised by the user.
    /// </summary>
    /// <param name="document">Affected source file.</param>
    /// <param name="methodDeclarationSyntax">Highlighted property declaration Syntax Node.</param>
    /// <param name="cancellationToken">Any fix is cancellable by the user, so we should support the cancellation token.</param>
    /// <returns>Clone of the solution with updates: renamed class.</returns>
    private static async Task<Document> AddVirtualKeyword(Document document,
        MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
    {
        var accessibilityModifier = methodDeclarationSyntax.Modifiers.LastOrDefault(modifier =>
            modifier.IsKind(SyntaxKind.PublicKeyword) ||
            modifier.IsKind(SyntaxKind.ProtectedKeyword) ||
            modifier.IsKind(SyntaxKind.InternalKeyword) ||
            modifier.IsKind(SyntaxKind.PrivateKeyword));

        var index = Math.Max(0, methodDeclarationSyntax.Modifiers.IndexOf(accessibilityModifier) + 1);

        var newMethodDeclarationSyntax = methodDeclarationSyntax.WithModifiers(
            SyntaxFactory.TokenList(methodDeclarationSyntax.Modifiers.Insert(index, SyntaxFactory.Token(SyntaxKind.VirtualKeyword))));

        var root = await document.GetSyntaxRootAsync(cancellationToken);

        if (root is null)
        {
            return document;
        }

        return document.WithSyntaxRoot(root.ReplaceNode(methodDeclarationSyntax, newMethodDeclarationSyntax));
    }
}