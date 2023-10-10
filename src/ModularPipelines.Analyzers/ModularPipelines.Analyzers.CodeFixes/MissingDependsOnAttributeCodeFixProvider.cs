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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingDependsOnAttributeCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class MissingDependsOnAttributeCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(MissingDependsOnAttributeAnalyzer.DiagnosticId);

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
        var declaration = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

        if (declaration is null)
        {
            return;
        }

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

        var syntaxTree = await context.Document.GetSyntaxTreeAsync(cancellationToken);

        var documentRoot = (await document.GetSyntaxRootAsync(cancellationToken))!;

        var name = context.Diagnostics.First().Properties["Name"]!;

        var attributes = typeDecl.AttributeLists.Add(
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(CreateDependsOnAttribute(name, syntaxTree!)))
                .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed)
                .NormalizeWhitespace());

        return document.WithSyntaxRoot(
            documentRoot
                .ReplaceNode(typeDecl, typeDecl.WithAttributeLists(attributes).WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed))
                .AddUsings()
                .NormalizeWhitespace()
        );
    }

    private static AttributeSyntax CreateDependsOnAttribute(string name, SyntaxTree syntaxTree)
    {
        var cSharpParseOptions = (CSharpParseOptions) syntaxTree.Options;

        if (cSharpParseOptions.LanguageVersion.MapSpecifiedToEffectiveVersion() >= (LanguageVersion) 1100)
        {
            return SyntaxFactory.Attribute(SyntaxFactory.ParseName($"DependsOn<{name}>"));
        }

        return SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("DependsOn"))
            .WithArgumentList(
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(name))
                        )
                    )
                )
            );
    }
}
