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
    private static readonly FixAllProvider DocumentFixAllProvider =
        FixAllProvider.Create(FixAllInDocumentAsync);

    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        [LoggerInConstructorAnalyzer.DiagnosticId];

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => DocumentFixAllProvider;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var fix = await TryCreateFixAsync(
            context.Document,
            diagnostic,
            context.CancellationToken).ConfigureAwait(false);
        if (fix is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.LoggerInConstructorCodeFixTitle,
                cancellationToken => ReplaceWithContextLoggerAsync(
                    context.Document,
                    [fix],
                    cancellationToken),
                nameof(CodeFixResources.LoggerInConstructorCodeFixTitle)),
            diagnostic);
    }

    private static async Task<LoggerFix?> TryCreateFixAsync(
        Document document,
        Diagnostic diagnostic,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        var parameter = root?.FindNode(diagnostic.Location.SourceSpan)
            .FirstAncestorOrSelf<ParameterSyntax>();

        if (parameter is null
            || parameter.AttributeLists.Count != 0
            || parameter.FirstAncestorOrSelf<ConstructorDeclarationSyntax>() is not { } constructor
            || constructor.ParameterList.ContainsDirectives
            || parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>() is not { } containingType
            || semanticModel?.GetDeclaredSymbol(parameter, cancellationToken) is not IParameterSymbol parameterSymbol
            || semanticModel.GetDeclaredSymbol(constructor, cancellationToken) is not IMethodSymbol constructorSymbol
            || await HasExplicitReferencesAsync(
                constructorSymbol,
                document.Project.Solution,
                cancellationToken).ConfigureAwait(false)
            || !TryCreateFix(
                containingType,
                constructor,
                constructorSymbol,
                parameterSymbol,
                semanticModel,
                cancellationToken,
                out var fieldDeclaration,
                out var assignmentStatement,
                out var loggerReplacements))
        {
            return null;
        }

        return new LoggerFix(
            constructor,
            constructorSymbol,
            parameter,
            parameterSymbol,
            fieldDeclaration,
            assignmentStatement,
            loggerReplacements);
    }

    private static async Task<Document?> FixAllInDocumentAsync(
        FixAllContext context,
        Document document,
        ImmutableArray<Diagnostic> diagnostics)
    {
        var fixes = ImmutableArray.CreateBuilder<LoggerFix>();
        foreach (var diagnostic in diagnostics)
        {
            var fix = await TryCreateFixAsync(
                document,
                diagnostic,
                context.CancellationToken).ConfigureAwait(false);
            if (fix is not null)
            {
                fixes.Add(fix);
            }
        }

        return await ReplaceWithContextLoggerAsync(
            document,
            fixes.ToImmutable(),
            context.CancellationToken).ConfigureAwait(false);
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

        if (containingType.ContainsConditionalDirectives()
            || containingType.Modifiers.Any(SyntaxKind.PartialKeyword)
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
        return WouldOverlapSiblingConstructor(constructor, [removedParameter]);
    }

    private static bool WouldOverlapSiblingConstructor(
        IMethodSymbol constructor,
        IReadOnlyCollection<IParameterSymbol> removedParameters)
    {
        var remainingParameters = constructor.Parameters
            .Where(parameter => !removedParameters.Any(removedParameter =>
                SymbolEqualityComparer.Default.Equals(parameter, removedParameter)))
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
            || !IsCurrentInstanceFieldReference(assignment.Left)
            || field.IsStatic
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

    private static bool IsCurrentInstanceFieldReference(ExpressionSyntax expression)
    {
        return expression is IdentifierNameSyntax
            or MemberAccessExpressionSyntax
        {
            Expression: ThisExpressionSyntax,
        };
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
            "Microsoft.Extensions.Logging.ILogger");

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
        ImmutableArray<LoggerFix> fixes,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var applicableFixes = GetApplicableFixes(fixes);

        foreach (var replacement in applicableFixes.SelectMany(static fix => fix.LoggerReplacements))
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

        foreach (var fieldDeclaration in applicableFixes
                     .Select(static fix => fix.FieldDeclaration)
                     .OfType<FieldDeclarationSyntax>()
                     .Distinct())
        {
            editor.RemoveNode(fieldDeclaration);
        }

        foreach (var constructorFixes in applicableFixes.GroupBy(static fix => fix.Constructor))
        {
            var constructor = constructorFixes.Key;
            var parameters = constructorFixes
                .Select(static fix => fix.Parameter)
                .ToImmutableHashSet();
            var assignmentStatements = constructorFixes
                .Select(static fix => fix.AssignmentStatement)
                .OfType<ExpressionStatementSyntax>()
                .ToImmutableHashSet();
            var removeConstructor = CanRemoveConstructor(
                constructor,
                parameters,
                assignmentStatements);

            if (removeConstructor)
            {
                editor.RemoveNode(constructor);
                continue;
            }

            foreach (var assignmentStatement in assignmentStatements)
            {
                editor.RemoveNode(assignmentStatement);
            }

            var remainingParameters = constructor.ParameterList.Parameters;
            foreach (var parameter in parameters)
            {
                remainingParameters = remainingParameters.Remove(parameter);
            }

            editor.ReplaceNode(
                constructor.ParameterList,
                constructor.ParameterList
                    .WithParameters(remainingParameters)
                    .WithAdditionalAnnotations(Formatter.Annotation));
        }

        return editor.GetChangedDocument();
    }

    private static ImmutableArray<LoggerFix> GetApplicableFixes(
        ImmutableArray<LoggerFix> fixes)
    {
        var applicableFixes = ImmutableArray.CreateBuilder<LoggerFix>();
        foreach (var constructorFixes in fixes.GroupBy(static fix => fix.Constructor))
        {
            var selectedFixes = new List<LoggerFix>();
            foreach (var fix in constructorFixes)
            {
                var removedParameters = selectedFixes
                    .Select(static selectedFix => selectedFix.ParameterSymbol)
                    .Append(fix.ParameterSymbol)
                    .ToArray();
                if (!WouldOverlapSiblingConstructor(
                        fix.ConstructorSymbol,
                        removedParameters))
                {
                    selectedFixes.Add(fix);
                }
            }

            applicableFixes.AddRange(selectedFixes);
        }

        return applicableFixes.ToImmutable();
    }

    private static bool CanRemoveConstructor(
        ConstructorDeclarationSyntax constructor,
        ImmutableHashSet<ParameterSyntax> parameters,
        ImmutableHashSet<ExpressionStatementSyntax> assignmentStatements)
    {
        var remainingStatements = constructor.Body?.Statements.Count - assignmentStatements.Count;
        return constructor.ParameterList.Parameters.Count == parameters.Count
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

    private sealed class LoggerFix(
        ConstructorDeclarationSyntax constructor,
        IMethodSymbol constructorSymbol,
        ParameterSyntax parameter,
        IParameterSymbol parameterSymbol,
        FieldDeclarationSyntax? fieldDeclaration,
        ExpressionStatementSyntax? assignmentStatement,
        ImmutableArray<LoggerReplacement> loggerReplacements)
    {
        public ConstructorDeclarationSyntax Constructor { get; } = constructor;

        public IMethodSymbol ConstructorSymbol { get; } = constructorSymbol;

        public ParameterSyntax Parameter { get; } = parameter;

        public IParameterSymbol ParameterSymbol { get; } = parameterSymbol;

        public FieldDeclarationSyntax? FieldDeclaration { get; } = fieldDeclaration;

        public ExpressionStatementSyntax? AssignmentStatement { get; } = assignmentStatement;

        public ImmutableArray<LoggerReplacement> LoggerReplacements { get; } = loggerReplacements;
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
