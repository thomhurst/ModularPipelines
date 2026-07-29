using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Formatting;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LoggerInConstructorCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public sealed class LoggerInConstructorCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [LoggerInConstructorAnalyzer.DiagnosticId];

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
            || constructor.ParameterList.ContainsDirectives
            || parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>() is not { } containingType
            || semanticModel?.GetDeclaredSymbol(parameter, context.CancellationToken) is not IParameterSymbol parameterSymbol
            || semanticModel.GetDeclaredSymbol(constructor, context.CancellationToken) is not IMethodSymbol constructorSymbol
            || await HasExplicitReferencesAsync(
                constructorSymbol,
                context.Document.Project.Solution,
                context.CancellationToken).ConfigureAwait(false)
            || !TryCreateFix(
                containingType,
                constructor,
                constructorSymbol,
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
        IMethodSymbol constructorSymbol,
        IParameterSymbol parameter,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        out FieldDeclarationSyntax? fieldDeclaration,
        out ExpressionStatementSyntax? assignmentStatement,
        out ImmutableArray<LoggerReplacement> loggerReplacements)
    {
        fieldDeclaration = null;
        assignmentStatement = null;
        loggerReplacements = [];

        if (containingType.Modifiers.Any(SyntaxKind.PartialKeyword)
            || containingType.Parent is TypeDeclarationSyntax)
        {
            return false;
        }

        if (containingType.DescendantNodes()
                .OfType<ConstructorInitializerSyntax>()
                .Where(initializer => initializer.ThisOrBaseKeyword.IsKind(SyntaxKind.ThisKeyword))
                .Any(initializer => SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetSymbolInfo(initializer, cancellationToken).Symbol,
                    constructorSymbol))
            || WouldOverlapSiblingConstructor(constructorSymbol, parameter))
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
        var loggerStorage = GetLoggerStorage(
            containingType,
            parameterReference,
            semanticModel,
            cancellationToken);
        if (loggerStorage is null)
        {
            return false;
        }

        if (loggerStorage.AssignmentStatement.Parent != constructor.Body
            || loggerStorage.FieldDeclaration.ContainsDirectives
            || loggerStorage.AssignmentStatement.ContainsDirectives)
        {
            return false;
        }

        var replacements = CreateLoggerReplacements(
            containingType,
            loggerStorage,
            semanticModel,
            cancellationToken);
        if (replacements is null)
        {
            return false;
        }

        fieldDeclaration = loggerStorage.FieldDeclaration;
        assignmentStatement = loggerStorage.AssignmentStatement;
        loggerReplacements = replacements.Value;
        return true;
    }

    private static async Task<bool> HasExplicitReferencesAsync(
        IMethodSymbol constructor,
        Solution solution,
        CancellationToken cancellationToken)
    {
        var references = await SymbolFinder.FindReferencesAsync(
            constructor,
            solution,
            cancellationToken).ConfigureAwait(false);

        return references.Any(static reference => reference.Locations.Any());
    }

    private static bool WouldOverlapSiblingConstructor(
        IMethodSymbol constructor,
        IParameterSymbol removedParameter)
    {
        var remainingParameters = constructor.Parameters
            .Where(parameter => !SymbolEqualityComparer.Default.Equals(parameter, removedParameter))
            .ToArray();
        var (remainingMinimum, remainingMaximum) = GetCallableArity(remainingParameters);

        return constructor.ContainingType.InstanceConstructors
            .Where(other => !SymbolEqualityComparer.Default.Equals(other, constructor))
            .Select(static other => GetCallableArity(other.Parameters))
            .Any(otherArity =>
                remainingMinimum <= otherArity.Maximum
                && otherArity.Minimum <= remainingMaximum);
    }

    private static (int Minimum, int Maximum) GetCallableArity(
        IReadOnlyList<IParameterSymbol> parameters)
    {
        var minimum = parameters.Count(static parameter =>
            !parameter.IsOptional && !parameter.IsParams);
        var maximum = parameters.Any(static parameter => parameter.IsParams)
            ? int.MaxValue
            : parameters.Count;
        return (minimum, maximum);
    }

    private static LoggerStorage? GetLoggerStorage(
        TypeDeclarationSyntax containingType,
        IdentifierNameSyntax parameterReference,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (parameterReference.Parent is not AssignmentExpressionSyntax assignment
            || !assignment.IsKind(SyntaxKind.SimpleAssignmentExpression)
            || assignment.Parent is not ExpressionStatementSyntax assignmentStatement
            || assignment.Right != parameterReference
            || semanticModel.GetSymbolInfo(assignment.Left, cancellationToken).Symbol is not IFieldSymbol field
            || field.DeclaredAccessibility != Accessibility.Private
            || !IsLogger(field.Type))
        {
            return null;
        }

        var fieldVariable = containingType.DescendantNodes()
            .OfType<VariableDeclaratorSyntax>()
            .FirstOrDefault(variable => SymbolEqualityComparer.Default.Equals(
                semanticModel.GetDeclaredSymbol(variable, cancellationToken),
                field));
        var fieldDeclaration = fieldVariable?.Parent?.Parent as FieldDeclarationSyntax;

        return fieldVariable?.Initializer is null
               && fieldDeclaration is { AttributeLists.Count: 0 }
               && fieldDeclaration.Declaration.Variables.Count == 1
            ? new LoggerStorage(field, fieldDeclaration, assignmentStatement, assignment)
            : null;
    }

    private static ImmutableArray<LoggerReplacement>? CreateLoggerReplacements(
        TypeDeclarationSyntax containingType,
        LoggerStorage loggerStorage,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var replacements = ImmutableArray.CreateBuilder<LoggerReplacement>();
        foreach (var fieldReference in GetReferences(
                     containingType,
                     loggerStorage.Field,
                     semanticModel,
                     cancellationToken))
        {
            if (loggerStorage.Assignment.Left.Span.Contains(fieldReference.Span))
            {
                continue;
            }

            var nodeToReplace = GetLoggerExpression(fieldReference);
            if (nodeToReplace.Parent is not MemberAccessExpressionSyntax { Expression: var expression }
                || expression != nodeToReplace
                || nodeToReplace.FindVisibleModuleContextParameter(
                    semanticModel,
                    cancellationToken) is not { } contextParameter
                || !CanReplaceLoggerReceiver(
                    nodeToReplace,
                    contextParameter.Identifier,
                    semanticModel,
                    cancellationToken))
            {
                return null;
            }

            replacements.Add(new LoggerReplacement(nodeToReplace, contextParameter.Identifier));
        }

        return replacements.ToImmutable();
    }

    private static bool CanReplaceLoggerReceiver(
        ExpressionSyntax loggerExpression,
        SyntaxToken contextParameterIdentifier,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (loggerExpression.Parent is not MemberAccessExpressionSyntax memberAccess)
        {
            return false;
        }

        var requiredReceiverType = semanticModel.GetSymbolInfo(memberAccess, cancellationToken).Symbol switch
        {
            IMethodSymbol { ReducedFrom: { Parameters.Length: > 0 } reducedMethod } =>
                reducedMethod.Parameters[0].Type,
            IMethodSymbol method => method.ContainingType,
            IPropertySymbol property => property.ContainingType,
            IFieldSymbol field => field.ContainingType,
            IEventSymbol @event => @event.ContainingType,
            _ => null,
        };
        var contextLoggerType = semanticModel.Compilation.GetTypeByMetadataName(
            "ModularPipelines.Logging.IModuleLogger");

        if (requiredReceiverType is null
            || contextLoggerType is null
            || !semanticModel.Compilation
                .ClassifyCommonConversion(contextLoggerType, requiredReceiverType)
                .IsImplicit)
        {
            return false;
        }

        ExpressionSyntax expressionToBind =
            memberAccess.Parent is InvocationExpressionSyntax invocation
            && invocation.Expression == memberAccess
                ? invocation
                : memberAccess;
        var contextLogger = SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName(contextParameterIdentifier.WithoutTrivia()),
            SyntaxFactory.IdentifierName("Logger"));
        var rewrittenExpression = expressionToBind.ReplaceNode(
            loggerExpression,
            contextLogger);
        var originalSymbol = semanticModel.GetSymbolInfo(
            expressionToBind,
            cancellationToken).Symbol;
        var rewrittenSymbol = semanticModel.GetSpeculativeSymbolInfo(
            expressionToBind.SpanStart,
            rewrittenExpression,
            SpeculativeBindingOption.BindAsExpression).Symbol;

        return SymbolEqualityComparer.Default.Equals(originalSymbol, rewrittenSymbol);
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
        return
        [
            .. containingType.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Where(identifier => SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol,
                    symbol)),
        ];
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
        var removeConstructor = CanRemoveConstructor(constructor, assignmentStatement);

        foreach (var replacement in loggerReplacements)
        {
            editor.ReplaceNode(
                replacement.Node,
                SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(
                            replacement.ContextParameterIdentifier.WithoutTrivia()),
                        SyntaxFactory.IdentifierName("Logger"))
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

    private static bool CanRemoveConstructor(
        ConstructorDeclarationSyntax constructor,
        ExpressionStatementSyntax? assignmentStatement)
    {
        var removedStatementCount = assignmentStatement is null ? 0 : 1;
        var remainingStatements = constructor.Body?.Statements.Count - removedStatementCount;
        return constructor.ParameterList.Parameters.Count == 1
               && remainingStatements == 0
               && constructor.Initializer is null
               && constructor.AttributeLists.Count == 0
               && !constructor.ContainsDirectives
               && constructor.Modifiers.Any(SyntaxKind.PublicKeyword)
               && constructor.Parent is TypeDeclarationSyntax containingType
               && !containingType.Modifiers.Any(SyntaxKind.AbstractKeyword)
               && IsOnlyInstanceConstructor(constructor);
    }

    private static bool IsOnlyInstanceConstructor(ConstructorDeclarationSyntax constructor)
    {
        return constructor.Parent is TypeDeclarationSyntax containingType
               && !containingType.Members
                   .OfType<ConstructorDeclarationSyntax>()
                   .Any(other => other != constructor
                                 && !other.Modifiers.Any(SyntaxKind.StaticKeyword));
    }

    private sealed class LoggerReplacement(SyntaxNode node, SyntaxToken contextParameterIdentifier)
    {
        public SyntaxNode Node { get; } = node;

        public SyntaxToken ContextParameterIdentifier { get; } = contextParameterIdentifier;
    }

    private sealed class LoggerStorage(
        IFieldSymbol field,
        FieldDeclarationSyntax fieldDeclaration,
        ExpressionStatementSyntax assignmentStatement,
        AssignmentExpressionSyntax assignment)
    {
        public IFieldSymbol Field { get; } = field;

        public FieldDeclarationSyntax FieldDeclaration { get; } = fieldDeclaration;

        public ExpressionStatementSyntax AssignmentStatement { get; } = assignmentStatement;

        public AssignmentExpressionSyntax Assignment { get; } = assignment;
    }
}
