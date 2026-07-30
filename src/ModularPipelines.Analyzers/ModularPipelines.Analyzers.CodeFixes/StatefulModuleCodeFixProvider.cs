using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StatefulModuleCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class StatefulModuleCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [StatefulModuleAnalyzer.DiagnosticId];

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
            || fieldDeclaration.Modifiers.Any(SyntaxKind.VolatileKeyword)
            || fieldDeclaration.Modifiers.Any(SyntaxKind.RequiredKeyword)
            || variable.FirstAncestorOrSelf<TypeDeclarationSyntax>() is not { } containingType
            || containingType.Modifiers.Any(SyntaxKind.PartialKeyword)
            || containingType.Parent is TypeDeclarationSyntax
            || semanticModel?.GetDeclaredSymbol(variable, context.CancellationToken) is not IFieldSymbol field
            || field.DeclaredAccessibility != Accessibility.Private
            || !IsDeeplyImmutable(field.Type)
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

    private static bool IsDeeplyImmutable(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol nullableType
            && nullableType.OriginalDefinition.SpecialType
            == SpecialType.System_Nullable_T)
        {
            return IsDeeplyImmutable(nullableType.TypeArguments[0]);
        }

        return type.SpecialType == SpecialType.System_String
               || type.TypeKind == TypeKind.Enum
               || (type.IsValueType && type.SpecialType != SpecialType.None);
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
                IsRefEscape(identifier)
                || (IsWrite(identifier, field, semanticModel, cancellationToken)
                    && !IsCurrentInstanceWriteWithinConstructor(
                        identifier,
                        containingType,
                        semanticModel,
                        cancellationToken)));
    }

    private static bool IsRefEscape(IdentifierNameSyntax identifier)
    {
        return identifier.FirstAncestorOrSelf<RefExpressionSyntax>() is not null
               || identifier.FirstAncestorOrSelf<MakeRefExpressionSyntax>() is not null
               || identifier.FirstAncestorOrSelf<PrefixUnaryExpressionSyntax>()
                      ?.IsKind(SyntaxKind.AddressOfExpression) == true;
    }

    private static bool IsCurrentInstanceWriteWithinConstructor(
        IdentifierNameSyntax identifier,
        TypeDeclarationSyntax containingType,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var containingCallable = identifier.Ancestors().FirstOrDefault(node =>
            node is BaseMethodDeclarationSyntax
                or LocalFunctionStatementSyntax
                or AnonymousFunctionExpressionSyntax
                or AccessorDeclarationSyntax);
        SyntaxNode fieldReferenceNode = identifier.Parent is MemberAccessExpressionSyntax memberAccess
                                        && memberAccess.Name == identifier
            ? memberAccess
            : identifier;
        var fieldReference = semanticModel.GetOperation(fieldReferenceNode, cancellationToken)
            as IFieldReferenceOperation;

        return containingCallable is ConstructorDeclarationSyntax constructor
               && constructor.Parent == containingType
               && fieldReference?.Instance is IInstanceReferenceOperation
               {
                   ReferenceKind: InstanceReferenceKind.ContainingTypeInstance,
               };
    }

    private static bool IsWrite(
        IdentifierNameSyntax identifier,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (IsByRefExtensionReceiver(identifier, semanticModel, cancellationToken))
        {
            return true;
        }

        var assignment = identifier.FirstAncestorOrSelf<AssignmentExpressionSyntax>();
        if (assignment is not null
            && IsAssignmentWrite(
                identifier,
                assignment,
                field,
                semanticModel,
                cancellationToken))
        {
            return true;
        }

        return IsUnaryWrite(identifier, field, semanticModel, cancellationToken)
               || IsRefArgumentWrite(identifier, field, semanticModel, cancellationToken);
    }

    private static bool IsAssignmentWrite(
        IdentifierNameSyntax identifier,
        AssignmentExpressionSyntax assignment,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (assignment.Left is TupleExpressionSyntax tuple)
        {
            return ContainsFieldTarget(tuple, field, semanticModel, cancellationToken);
        }

        return assignment.Left.Span.Contains(identifier.Span)
               && SymbolEqualityComparer.Default.Equals(
                   semanticModel.GetSymbolInfo(assignment.Left, cancellationToken).Symbol,
                   field);
    }

    private static bool IsUnaryWrite(
        IdentifierNameSyntax identifier,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
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

        return false;
    }

    private static bool IsRefArgumentWrite(
        IdentifierNameSyntax identifier,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var argument = identifier.FirstAncestorOrSelf<ArgumentSyntax>();
        if (argument is null)
        {
            return false;
        }

        var argumentOperation = semanticModel.GetOperation(argument, cancellationToken)
            as IArgumentOperation;
        return (!argument.RefKindKeyword.IsKind(SyntaxKind.None)
                || argumentOperation?.Parameter?.RefKind != RefKind.None)
               && SymbolEqualityComparer.Default.Equals(
                   semanticModel.GetSymbolInfo(argument.Expression, cancellationToken).Symbol,
                   field);
    }

    private static bool IsByRefExtensionReceiver(
        IdentifierNameSyntax identifier,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var invocation = identifier.Ancestors()
            .OfType<InvocationExpressionSyntax>()
            .FirstOrDefault(candidate =>
                candidate.Expression is MemberAccessExpressionSyntax memberAccess
                && memberAccess.Expression.Span.Contains(identifier.Span));

        return invocation is not null
               && semanticModel.GetSymbolInfo(invocation, cancellationToken).Symbol
                   is IMethodSymbol
               {
                   ReducedFrom: { Parameters.Length: > 0 } reducedFrom,
               }

               && reducedFrom.Parameters[0].RefKind != RefKind.None;
    }

    private static bool ContainsFieldTarget(
        TupleExpressionSyntax tuple,
        IFieldSymbol field,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        return tuple.Arguments.Any(argument =>
            argument.Expression is TupleExpressionSyntax nestedTuple
                ? ContainsFieldTarget(
                    nestedTuple,
                    field,
                    semanticModel,
                    cancellationToken)
                : SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetSymbolInfo(argument.Expression, cancellationToken).Symbol,
                    field));
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
