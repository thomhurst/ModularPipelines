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
        if (!diagnostic.Properties.TryGetValue("ModuleDeclarationStart", out var declarationStartValue)
            || !int.TryParse(declarationStartValue, out var declarationStart))
        {
            return;
        }

        // The analyzer records the enclosing module so nested helper diagnostics fix the module itself.
        var declaration = root
            .FindToken(declarationStart)
            .Parent?
            .AncestorsAndSelf()
            .OfType<TypeDeclarationSyntax>()
            .FirstOrDefault(typeDeclaration => typeDeclaration.SpanStart == declarationStart);

        if (declaration is null)
        {
            return;
        }

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: CodeFixResources.MissingDependsOnAttributeCodeFixTitle,
                createChangedDocument: c => AddAttribute(context, declaration, c),
                equivalenceKey: nameof(CodeFixResources.MissingDependsOnAttributeCodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> AddAttribute(CodeFixContext context, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
    {
        var document = context.Document;

        var syntaxTree = await context.Document.GetSyntaxTreeAsync(cancellationToken);

        var documentRoot = (await document.GetSyntaxRootAsync(cancellationToken))!;

        var diagnostic = context.Diagnostics.First();
        var name = diagnostic.Properties["Name"]!;
        var isOptional = diagnostic.Properties.TryGetValue("Optional", out var optionalValue)
                         && bool.TryParse(optionalValue, out var optional)
                         && optional;
        var endOfLine = documentRoot
            .DescendantTrivia()
            .FirstOrDefault(trivia => trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            .ToFullString();

        if (string.IsNullOrEmpty(endOfLine))
        {
            endOfLine = Environment.NewLine;
        }

        var attributes = typeDecl.AttributeLists.Add(
            SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(CreateDependsOnAttribute(name, syntaxTree!, isOptional)))
                .WithTrailingTrivia(SyntaxFactory.EndOfLine(endOfLine))
                .NormalizeWhitespace(eol: endOfLine));

        return document.WithSyntaxRoot(
            documentRoot
                .ReplaceNode(typeDecl, typeDecl.WithAttributeLists(attributes).WithTrailingTrivia(SyntaxFactory.EndOfLine(endOfLine)))
                .AddUsings()
                .NormalizeWhitespace(eol: endOfLine)
        );
    }

    private static AttributeSyntax CreateDependsOnAttribute(string name, SyntaxTree syntaxTree, bool isOptional)
    {
        var cSharpParseOptions = (CSharpParseOptions) syntaxTree.Options;
        AttributeSyntax attribute;

        if (cSharpParseOptions.LanguageVersion.MapSpecifiedToEffectiveVersion() >= (LanguageVersion) 1100)
        {
            attribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName($"DependsOn<{name}>"));
        }
        else
        {
            attribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("DependsOn"))
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

        if (!isOptional)
        {
            return attribute;
        }

        var optionalArgument = SyntaxFactory.AttributeArgument(
                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
            .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Optional")));
        var arguments = attribute.ArgumentList?.Arguments ?? default;

        return attribute.WithArgumentList(SyntaxFactory.AttributeArgumentList(arguments.Add(optionalArgument)));
    }
}
