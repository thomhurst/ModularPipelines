using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.Analyzers.Extensions;

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
                title: CodeFixResources.CodeFixTitle,
                createChangedDocument: c => AddAsync(context, declaration, c),
                equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> AddAsync(CodeFixContext context, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
    {
        var document = await context.GetDocumentWithReplacedNode(methodDeclarationSyntax, methodDeclarationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword)));

        var root = await document.GetSyntaxRootAsync(cancellationToken);
        
        foreach (var returnStatement in 
                 methodDeclarationSyntax.Body
                     ?.Statements
                     .Where(x => x.Kind() == SyntaxKind.ReturnStatement)
                     .Distinct()
                 ?? Array.Empty<StatementSyntax>())
        {
            var expressionSyntax = returnStatement.ChildNodes().OfType<ExpressionSyntax>().First()!;

            var awaitExpressionSyntax = SyntaxFactory.AwaitExpression(expressionSyntax).NormalizeWhitespace();

            var awaitedReturnStatement = returnStatement.ReplaceNode(expressionSyntax, awaitExpressionSyntax);
            
            root = root!.ReplaceNode(returnStatement, awaitedReturnStatement);
        }

        return document.WithSyntaxRoot(root!);
    }
}