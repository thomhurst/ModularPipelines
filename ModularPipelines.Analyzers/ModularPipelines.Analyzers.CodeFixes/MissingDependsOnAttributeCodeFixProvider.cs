using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingDependsOnAttributeCodeFixProvider)), Shared]
public class MissingDependsOnAttributeCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(MissingDependsOnAttributeAnalyzer.DiagnosticId); }
    }

    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the type declaration identified by the diagnostic.
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.CodeFixTitle,
                createChangedDocument: c => AddAttribute(context, declaration, c),
                equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> AddAttribute(CodeFixContext context, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
    {
        var document = context.Document;
        var documentRoot = await document.GetSyntaxRootAsync(cancellationToken);

        var name = context.Diagnostics.First().Properties["FullName"]!;

        var attributes = typeDecl.AttributeLists.Add(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("ModularPipelines.Attributes.DependsOnAttribute")
                        )
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(name))
                                    )
                                )
                            )
                        )
                )).NormalizeWhitespace());
            
        // TODO
        //documentRoot.DescendantNodes().OfType<UsingStatementSyntax>().Any(x => x.Statement.)
        
        return document.WithSyntaxRoot(
            documentRoot!.ReplaceNode(typeDecl, typeDecl.WithAttributeLists(attributes))
        );
    }
}