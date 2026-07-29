using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StatefulModuleCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class StatefulModuleCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(StatefulModuleAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var variable = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<VariableDeclaratorSyntax>();

        if (variable is null
            || variable.Parent?.Parent is not FieldDeclarationSyntax fieldDeclaration
            || fieldDeclaration.Declaration.Variables.Count != 1
            || variable.FirstAncestorOrSelf<TypeDeclarationSyntax>() is not { } containingType
            || semanticModel?.GetDeclaredSymbol(variable, context.CancellationToken) is not IFieldSymbol field
            || IsWrittenOutsideConstructor(
                containingType,
                field,
                semanticModel,
                context.CancellationToken))
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.StatefulModuleCodeFixTitle,
                cancellationToken => AddReadonlyAsync(
                    context.Document,
                    fieldDeclaration,
                    cancellationToken),
                nameof(CodeFixResources.StatefulModuleCodeFixTitle)),
            diagnostic);
    }

    private static bool IsWrittenOutsideConstructor(
        TypeDeclarationSyntax containingType,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        return containingType.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Where(identifier => SymbolEqualityComparer.Default.Equals(
                semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol,
                field))
            .Any(identifier =>
                IsWrite(identifier, field, semanticModel, cancellationToken)
                && identifier.FirstAncestorOrSelf<ConstructorDeclarationSyntax>() is null);
    }

    private static bool IsWrite(
        IdentifierNameSyntax identifier,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var assignment = identifier.FirstAncestorOrSelf<AssignmentExpressionSyntax>();
        if (assignment is not null
            && assignment.Left.Span.Contains(identifier.Span)
            && SymbolEqualityComparer.Default.Equals(
                semanticModel.GetSymbolInfo(assignment.Left, cancellationToken).Symbol,
                field))
        {
            return true;
        }

        var prefix = identifier.FirstAncestorOrSelf<PrefixUnaryExpressionSyntax>();
        if (prefix?.IsKind(SyntaxKind.PreIncrementExpression) == true
            || prefix?.IsKind(SyntaxKind.PreDecrementExpression) == true)
        {
            return SymbolEqualityComparer.Default.Equals(
                semanticModel.GetSymbolInfo(prefix.Operand, cancellationToken).Symbol,
                field);
        }

        var postfix = identifier.FirstAncestorOrSelf<PostfixUnaryExpressionSyntax>();
        if (postfix?.IsKind(SyntaxKind.PostIncrementExpression) == true
            || postfix?.IsKind(SyntaxKind.PostDecrementExpression) == true)
        {
            return SymbolEqualityComparer.Default.Equals(
                semanticModel.GetSymbolInfo(postfix.Operand, cancellationToken).Symbol,
                field);
        }

        var argument = identifier.FirstAncestorOrSelf<ArgumentSyntax>();
        return argument?.RefKindKeyword.IsKind(SyntaxKind.None) == false
               && SymbolEqualityComparer.Default.Equals(
                   semanticModel.GetSymbolInfo(argument.Expression, cancellationToken).Symbol,
                   field);
    }

    private static async Task<Document> AddReadonlyAsync(
        Document document,
        FieldDeclarationSyntax fieldDeclaration,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var newDeclaration = fieldDeclaration
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword))
            .WithAdditionalAnnotations(Formatter.Annotation);
        return document.WithSyntaxRoot(root.ReplaceNode(fieldDeclaration, newDeclaration));
    }
}
