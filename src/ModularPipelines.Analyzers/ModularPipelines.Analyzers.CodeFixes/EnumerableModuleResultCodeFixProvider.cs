using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumerableModuleResultCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class EnumerableModuleResultCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(EnumerableModuleResultAnalyzer.DiagnosticId);

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

        // Find the base type syntax identified by the diagnostic.
        var baseTypeSyntax = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<SimpleBaseTypeSyntax>().FirstOrDefault();

        if (baseTypeSyntax is null)
        {
            return;
        }

        // Register a code action to convert IEnumerable<T> to List<T>
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.EnumerableModuleResultToListCodeFixTitle,
                createChangedDocument: c => ReplaceEnumerableWithList(context, baseTypeSyntax, c),
                equivalenceKey: nameof(CodeFixResources.EnumerableModuleResultToListCodeFixTitle)),
            diagnostic);

        // Register a code action to convert IEnumerable<T> to T[]
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.EnumerableModuleResultToArrayCodeFixTitle,
                createChangedDocument: c => ReplaceEnumerableWithArray(context, baseTypeSyntax, c),
                equivalenceKey: nameof(CodeFixResources.EnumerableModuleResultToArrayCodeFixTitle)),
            diagnostic);
    }

    private static async Task<Document> ReplaceEnumerableWithList(CodeFixContext context, SimpleBaseTypeSyntax baseTypeSyntax, CancellationToken cancellationToken)
    {
        var document = context.Document;
        var documentRoot = (await document.GetSyntaxRootAsync(cancellationToken))!;

        if (baseTypeSyntax.Type is not GenericNameSyntax moduleGenericSyntax)
        {
            return document;
        }

        if (moduleGenericSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not GenericNameSyntax enumerableGenericSyntax)
        {
            return document;
        }

        // Get the inner type argument (the T in IEnumerable<T>)
        var innerTypeArgument = enumerableGenericSyntax.TypeArgumentList.Arguments.FirstOrDefault();
        if (innerTypeArgument is null)
        {
            return document;
        }

        // Create List<T>
        var listType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SingletonSeparatedList(innerTypeArgument)));

        // Create new Module<List<T>>
        var newModuleType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("Module"),
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(listType)));

        var newBaseTypeSyntax = SyntaxFactory.SimpleBaseType(newModuleType);

        var newRoot = documentRoot.ReplaceNode(baseTypeSyntax, newBaseTypeSyntax);

        // Add using for System.Collections.Generic if not present
        newRoot = AddUsingIfNeeded(newRoot, "System.Collections.Generic");

        return document.WithSyntaxRoot(newRoot);
    }

    private static async Task<Document> ReplaceEnumerableWithArray(CodeFixContext context, SimpleBaseTypeSyntax baseTypeSyntax, CancellationToken cancellationToken)
    {
        var document = context.Document;
        var documentRoot = (await document.GetSyntaxRootAsync(cancellationToken))!;

        if (baseTypeSyntax.Type is not GenericNameSyntax moduleGenericSyntax)
        {
            return document;
        }

        if (moduleGenericSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not GenericNameSyntax enumerableGenericSyntax)
        {
            return document;
        }

        // Get the inner type argument (the T in IEnumerable<T>)
        var innerTypeArgument = enumerableGenericSyntax.TypeArgumentList.Arguments.FirstOrDefault();
        if (innerTypeArgument is null)
        {
            return document;
        }

        // Create T[]
        var arrayType = SyntaxFactory.ArrayType(
            innerTypeArgument,
            SyntaxFactory.SingletonList(
                SyntaxFactory.ArrayRankSpecifier(
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                        SyntaxFactory.OmittedArraySizeExpression()))));

        // Create new Module<T[]>
        var newModuleType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("Module"),
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(arrayType)));

        var newBaseTypeSyntax = SyntaxFactory.SimpleBaseType(newModuleType);

        return document.WithSyntaxRoot(documentRoot.ReplaceNode(baseTypeSyntax, newBaseTypeSyntax));
    }

    private static SyntaxNode AddUsingIfNeeded(SyntaxNode documentRoot, string namespaceName)
    {
        if (documentRoot is not CompilationUnitSyntax compilationUnitSyntax)
        {
            return documentRoot;
        }

        if (compilationUnitSyntax.Usings.Any(u => u.Name?.ToFullString() == namespaceName))
        {
            return documentRoot;
        }

        return compilationUnitSyntax.AddUsings(
            SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName)));
    }
}
