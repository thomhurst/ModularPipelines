using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LoggerInConstructorCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class LoggerInConstructorCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(LoggerInConstructorAnalyzer.DiagnosticId);

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics.First();
        var parameter = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<ParameterSyntax>();

        if (parameter is null
            || parameter.FirstAncestorOrSelf<ConstructorDeclarationSyntax>() is not { } constructor
            || parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>() is not { } containingType
            || semanticModel?.GetDeclaredSymbol(parameter, context.CancellationToken) is not IParameterSymbol parameterSymbol
            || !TryCreateFix(
                containingType,
                constructor,
                parameterSymbol,
                semanticModel,
                context.CancellationToken,
                out var fieldDeclaration,
                out var assignmentStatement,
                out var loggerReplacements))
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.LoggerInConstructorCodeFixTitle,
                cancellationToken => ReplaceWithContextLoggerAsync(
                    context.Document,
                    constructor,
                    parameter,
                    fieldDeclaration,
                    assignmentStatement,
                    loggerReplacements,
                    cancellationToken),
                nameof(CodeFixResources.LoggerInConstructorCodeFixTitle)),
            diagnostic);
    }

    private static bool TryCreateFix(
        TypeDeclarationSyntax containingType,
        ConstructorDeclarationSyntax constructor,
        IParameterSymbol parameter,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        out FieldDeclarationSyntax? fieldDeclaration,
        out ExpressionStatementSyntax? assignmentStatement,
        out ImmutableArray<LoggerReplacement> loggerReplacements)
    {
        fieldDeclaration = null;
        assignmentStatement = null;
        loggerReplacements = ImmutableArray<LoggerReplacement>.Empty;

        if (containingType.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            return false;
        }

        var parameterReferences = GetReferences(containingType, parameter, semanticModel, cancellationToken);
        if (parameterReferences.Length == 0)
        {
            return true;
        }

        if (parameterReferences.Length != 1)
        {
            return false;
        }

        var parameterReference = parameterReferences[0];
        var assignment = parameterReference.Parent as AssignmentExpressionSyntax;
        var candidateAssignment = assignment?.Parent as ExpressionStatementSyntax;
        if (assignment?.IsKind(SyntaxKind.SimpleAssignmentExpression) != true
            || candidateAssignment is null
            || assignment.Right != parameterReference
            || semanticModel.GetSymbolInfo(assignment.Left, cancellationToken).Symbol is not IFieldSymbol field
            || !IsLogger(field.Type)
            || containingType.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .FirstOrDefault(variable => SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetDeclaredSymbol(variable, cancellationToken),
                    field)) is not { Parent.Parent: FieldDeclarationSyntax candidateField }
            || candidateField.Declaration.Variables.Count != 1)
        {
            return false;
        }

        var replacements = ImmutableArray.CreateBuilder<LoggerReplacement>();
        foreach (var fieldReference in GetReferences(containingType, field, semanticModel, cancellationToken))
        {
            if (assignment.Left.Span.Contains(fieldReference.Span))
            {
                continue;
            }

            var nodeToReplace = GetLoggerExpression(fieldReference);
            if (nodeToReplace.Parent is not MemberAccessExpressionSyntax { Expression: var expression }
                || expression != nodeToReplace
                || FindModuleContextParameter(nodeToReplace, semanticModel, cancellationToken) is not { } contextParameter)
            {
                return false;
            }

            replacements.Add(new LoggerReplacement(nodeToReplace, contextParameter.Identifier.ValueText));
        }

        fieldDeclaration = candidateField;
        assignmentStatement = candidateAssignment;
        loggerReplacements = replacements.ToImmutable();
        return true;
    }

    private static bool IsLogger(ITypeSymbol type)
    {
        return IsLoggerInterface(type)
               || type.AllInterfaces.Any(IsLoggerInterface);
    }

    private static bool IsLoggerInterface(ITypeSymbol type)
    {
        return type.OriginalDefinition.ToDisplayString() is
            "Microsoft.Extensions.Logging.ILogger"
            or "Microsoft.Extensions.Logging.ILogger<TCategoryName>";
    }

    private static ImmutableArray<IdentifierNameSyntax> GetReferences(
        TypeDeclarationSyntax containingType,
        ISymbol symbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        return containingType.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Where(identifier => SymbolEqualityComparer.Default.Equals(
                semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol,
                symbol))
            .ToImmutableArray();
    }

    private static ExpressionSyntax GetLoggerExpression(IdentifierNameSyntax fieldReference)
    {
        if (fieldReference.Parent is MemberAccessExpressionSyntax memberAccess
            && memberAccess.Expression is ThisExpressionSyntax
            && memberAccess.Name == fieldReference)
        {
            return memberAccess;
        }

        return fieldReference;
    }

    private static ParameterSyntax? FindModuleContextParameter(
        SyntaxNode node,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var moduleContextType = semanticModel.Compilation.GetTypeByMetadataName(
            "ModularPipelines.Context.IModuleContext");
        var method = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();

        return moduleContextType is null
            ? null
            : method?.ParameterList.Parameters.FirstOrDefault(parameter =>
                SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetTypeInfo(parameter.Type!, cancellationToken).Type,
                    moduleContextType));
    }

    private static async Task<Document> ReplaceWithContextLoggerAsync(
        Document document,
        ConstructorDeclarationSyntax constructor,
        ParameterSyntax parameter,
        FieldDeclarationSyntax? fieldDeclaration,
        ExpressionStatementSyntax? assignmentStatement,
        ImmutableArray<LoggerReplacement> loggerReplacements,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var remainingStatements = constructor.Body?.Statements.Count - (assignmentStatement is null ? 0 : 1);
        var removeConstructor = constructor.ParameterList.Parameters.Count == 1
                                && remainingStatements == 0
                                && constructor.Initializer is null
                                && constructor.Modifiers.Any(SyntaxKind.PublicKeyword);

        foreach (var replacement in loggerReplacements)
        {
            editor.ReplaceNode(
                replacement.Node,
                SyntaxFactory.ParseExpression($"{replacement.ContextParameterName}.Logger")
                    .WithTriviaFrom(replacement.Node)
                    .WithAdditionalAnnotations(Formatter.Annotation));
        }

        if (fieldDeclaration is not null)
        {
            editor.RemoveNode(fieldDeclaration);
        }

        if (assignmentStatement is not null && !removeConstructor)
        {
            editor.RemoveNode(assignmentStatement);
        }

        if (removeConstructor)
        {
            editor.RemoveNode(constructor);
        }
        else
        {
            editor.ReplaceNode(
                constructor.ParameterList,
                constructor.ParameterList.WithParameters(
                    constructor.ParameterList.Parameters.Remove(parameter)));
        }

        return editor.GetChangedDocument();
    }

    private sealed class LoggerReplacement(SyntaxNode node, string contextParameterName)
    {
        public SyntaxNode Node { get; } = node;

        public string ContextParameterName { get; } = contextParameterName;
    }
}
