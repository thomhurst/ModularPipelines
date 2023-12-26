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
                title: CodeFixResources.AsyncModuleCodeFixTitle,
                createChangedDocument: c => AddAsync(context, declaration, c),
                equivalenceKey: nameof(CodeFixResources.AsyncModuleCodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> AddAsync(CodeFixContext context, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
    {
        var document = context.Document;

        var newMethodDeclarationSyntax = methodDeclarationSyntax;

        var returnStatements = methodDeclarationSyntax.Body
                                    ?.DescendantNodes()
                                    .OfType<ReturnStatementSyntax>()
                                    .ToArray()
                                ?? Array.Empty<ReturnStatementSyntax>();
        
        foreach (var returnStatement in returnStatements)
        {
            if (returnStatement == null)
            {
                continue;
            }
            
            var expressionSyntax = returnStatement.ChildNodes().OfType<ExpressionSyntax>().First()!;

            if (await IsTaskFromResult(expressionSyntax, context)
                || await IsAsTaskExtension(expressionSyntax, context))
            {
                var firstInnerExpression = expressionSyntax.ChildNodes().OfType<ArgumentListSyntax>().First().Arguments.First();
                newMethodDeclarationSyntax = newMethodDeclarationSyntax.ReplaceNode(expressionSyntax, firstInnerExpression);
                continue;
            }

            var awaitExpressionSyntax = SyntaxFactory.AwaitExpression(expressionSyntax).NormalizeWhitespace();

            var awaitedReturnStatement = returnStatement.ReplaceNode(expressionSyntax, awaitExpressionSyntax);
            
            newMethodDeclarationSyntax = newMethodDeclarationSyntax.ReplaceNode(returnStatement, awaitedReturnStatement);
        }

        return await document
            .WithReplacedNode(methodDeclarationSyntax,
                newMethodDeclarationSyntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
            );
    }

    private async Task<bool> IsTaskFromResult(ExpressionSyntax expressionSyntax, CodeFixContext context)
    {
        if (expressionSyntax is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return false;
        }
        
        var semanticModel = await context.Document.GetSemanticModelAsync();
        
        var symbol = semanticModel.GetSymbolInfo(invocationExpressionSyntax).Symbol;

        if (symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        return
            methodSymbol.Name == "FromResult"
            && methodSymbol.ContainingType.Name == "Task"
            && methodSymbol.ContainingNamespace.ToDisplayString() == "System.Threading.Tasks";
    }
    
    private async Task<bool> IsAsTaskExtension(ExpressionSyntax expressionSyntax, CodeFixContext context)
    {
        if (expressionSyntax is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return false;
        }
        
        var semanticModel = await context.Document.GetSemanticModelAsync();
        
        var symbol = semanticModel.GetSymbolInfo(invocationExpressionSyntax).Symbol;

        if (symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        return
            methodSymbol.Name == "AsTask"
            && methodSymbol.ContainingType.Name == "TaskExtensions"
            && methodSymbol.ContainingNamespace.ToDisplayString() == "ModularPipelines.Extensions";
    }
}