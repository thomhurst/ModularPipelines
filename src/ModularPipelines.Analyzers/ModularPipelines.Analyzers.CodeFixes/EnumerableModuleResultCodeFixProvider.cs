using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumerableModuleResultCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class EnumerableModuleResultCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [EnumerableModuleResultAnalyzer.DiagnosticId];

    /// <inheritdoc/>
    public sealed override FixAllProvider GetFixAllProvider()
    {
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

        // Find the base type syntax identified by the diagnostic.
        var baseTypeSyntax = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<SimpleBaseTypeSyntax>().FirstOrDefault();

        if (baseTypeSyntax is null)
        {
            return;
        }

        var replacement = await GetReplacementContext(
                context.Document,
                baseTypeSyntax,
                context.CancellationToken).ConfigureAwait(false);
        if (replacement is null)
        {
            return;
        }

        if (await HasCompatibleExecutionOverride(
                replacement,
                ReplacementType.List,
                context.CancellationToken).ConfigureAwait(false))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.EnumerableModuleResultToListCodeFixTitle,
                    createChangedDocument: c => ReplaceEnumerableWithList(context, baseTypeSyntax, c),
                    equivalenceKey: nameof(CodeFixResources.EnumerableModuleResultToListCodeFixTitle)),
                diagnostic);
        }

        if (await HasCompatibleExecutionOverride(
                replacement,
                ReplacementType.Array,
                context.CancellationToken).ConfigureAwait(false))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.EnumerableModuleResultToArrayCodeFixTitle,
                    createChangedDocument: c => ReplaceEnumerableWithArray(context, baseTypeSyntax, c),
                    equivalenceKey: nameof(CodeFixResources.EnumerableModuleResultToArrayCodeFixTitle)),
                diagnostic);
        }
    }

    private static async Task<Document> ReplaceEnumerableWithList(CodeFixContext context, SimpleBaseTypeSyntax baseTypeSyntax, CancellationToken cancellationToken)
    {
        var replacement = await GetReplacementContext(
            context.Document,
            baseTypeSyntax,
            cancellationToken).ConfigureAwait(false);
        if (replacement is null)
        {
            return context.Document;
        }

        var listType = SyntaxFactory.QualifiedName(
            SyntaxFactory.ParseName("global::System.Collections.Generic"),
            SyntaxFactory.GenericName(
                SyntaxFactory.Identifier("List"),
                replacement.EnumerableGenericType.TypeArgumentList));
        var newRoot = replacement.DocumentRoot.ReplaceNode(
            replacement.EnumerableType,
            PreserveNullableAnnotation(replacement, listType)
                .WithTriviaFrom(replacement.EnumerableType));

        return context.Document.WithSyntaxRoot(newRoot);
    }

    private static async Task<Document> ReplaceEnumerableWithArray(CodeFixContext context, SimpleBaseTypeSyntax baseTypeSyntax, CancellationToken cancellationToken)
    {
        var replacement = await GetReplacementContext(
            context.Document,
            baseTypeSyntax,
            cancellationToken).ConfigureAwait(false);
        if (replacement is null)
        {
            return context.Document;
        }

        var arrayType = SyntaxFactory.ArrayType(
            replacement.ElementType,
            SyntaxFactory.SingletonList(
                SyntaxFactory.ArrayRankSpecifier(
                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                        SyntaxFactory.OmittedArraySizeExpression()))));

        return context.Document.WithSyntaxRoot(replacement.DocumentRoot.ReplaceNode(
            replacement.EnumerableType,
            PreserveNullableAnnotation(replacement, arrayType)
                .WithTriviaFrom(replacement.EnumerableType)));
    }

    private static TypeSyntax PreserveNullableAnnotation(
        ReplacementContext replacement,
        TypeSyntax replacementType)
    {
        return replacement.EnumerableType is NullableTypeSyntax nullableType
            ? SyntaxFactory.NullableType(replacementType, nullableType.QuestionToken)
            : replacementType;
    }

    private static async Task<ReplacementContext?> GetReplacementContext(
        Document document,
        SimpleBaseTypeSyntax baseType,
        CancellationToken cancellationToken)
    {
        var documentRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        var enumerableType = semanticModel?.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.IEnumerable);
        if (documentRoot is null || semanticModel is null || enumerableType is null)
        {
            return null;
        }

        if (semanticModel.GetTypeInfo(baseType.Type, cancellationToken).Type
                is not INamedTypeSymbol baseTypeSymbol)
        {
            return null;
        }

        var candidate = GetModuleResultTypeSyntax(
            baseType,
            baseTypeSymbol,
            semanticModel,
            cancellationToken);
        if (candidate is null)
        {
            return null;
        }

        if (semanticModel.GetTypeInfo(candidate, cancellationToken).Type
                is not INamedTypeSymbol candidateType
            || !SymbolEqualityComparer.Default.Equals(
                candidateType.OriginalDefinition,
                enumerableType))
        {
            return null;
        }

        var enumerableGenericType = GetEnumerableGenericTypeSyntax(
            candidate,
            enumerableType,
            semanticModel,
            cancellationToken);
        var elementType = enumerableGenericType?.TypeArgumentList.Arguments.FirstOrDefault();
        if (enumerableGenericType is null
            || elementType is null
            || semanticModel.GetTypeInfo(elementType, cancellationToken).Type
                is not ITypeSymbol elementTypeSymbol)
        {
            return null;
        }

        return new ReplacementContext(
            document,
            documentRoot,
            semanticModel,
            candidate,
            enumerableGenericType,
            elementType,
            elementTypeSymbol);
    }

    private static TypeSyntax? GetModuleResultTypeSyntax(
        SimpleBaseTypeSyntax baseType,
        INamedTypeSymbol baseTypeSymbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var moduleType = semanticModel.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.Module);
        if (moduleType is null
            || GetModuleResultTypeParameterOrdinal(baseTypeSymbol, moduleType) is not int ordinal)
        {
            return null;
        }

        var baseGenericName = baseType.Type
            .DescendantNodesAndSelf()
            .OfType<GenericNameSyntax>()
            .FirstOrDefault(candidate => SymbolEqualityComparer.Default.Equals(
                semanticModel.GetTypeInfo(candidate, cancellationToken).Type,
                baseTypeSymbol));
        return baseGenericName is not null
            && ordinal < baseGenericName.TypeArgumentList.Arguments.Count
                ? baseGenericName.TypeArgumentList.Arguments[ordinal]
                : null;
    }

    private static GenericNameSyntax? GetEnumerableGenericTypeSyntax(
        TypeSyntax candidate,
        INamedTypeSymbol enumerableType,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        return candidate
            .DescendantNodesAndSelf()
            .OfType<GenericNameSyntax>()
            .FirstOrDefault(generic => semanticModel.GetTypeInfo(generic, cancellationToken).Type
                    is INamedTypeSymbol genericType
                && SymbolEqualityComparer.Default.Equals(
                    genericType.OriginalDefinition,
                    enumerableType));
    }

    private static async Task<bool> HasCompatibleExecutionOverride(
        ReplacementContext replacement,
        ReplacementType replacementType,
        CancellationToken cancellationToken)
    {
        var containingTypeSymbol = replacement.EnumerableType
            .Ancestors()
            .OfType<TypeDeclarationSyntax>()
            .Select(type => replacement.SemanticModel.GetDeclaredSymbol(type))
            .OfType<INamedTypeSymbol>()
            .FirstOrDefault();
        if (containingTypeSymbol is null)
        {
            return false;
        }

        var derivedTypes = await SymbolFinder.FindDerivedClassesAsync(
                containingTypeSymbol,
                replacement.Document.Project.Solution,
                transitive: true,
                projects: null,
                cancellationToken)
            .ConfigureAwait(false);
        var affectedTypes = derivedTypes.Prepend(containingTypeSymbol).ToArray();
        if (affectedTypes.Any(HasResultDependentHookOverride))
        {
            return false;
        }

        var executionOverrides = affectedTypes
            .SelectMany(GetExecutionOverrides)
            .ToArray();
        if (executionOverrides.Length == 0)
        {
            return true;
        }

        var expectedResultType = GetExpectedResultType(replacement, replacementType);
        return expectedResultType is not null
            && executionOverrides.All(method => SymbolEqualityComparer.Default.Equals(
                GetExecutionResultType(method.ReturnType),
                expectedResultType));
    }

    private static IMethodSymbol[] GetExecutionOverrides(INamedTypeSymbol containingTypeSymbol)
    {
        return [.. containingTypeSymbol
            .GetMembers()
            .OfType<IMethodSymbol>()
            .Where(method => method.OverriddenMethod is not null
                && method.Name is "Execute" or "ExecuteAsync")];
    }

    private static bool HasResultDependentHookOverride(INamedTypeSymbol containingTypeSymbol)
    {
        return containingTypeSymbol
            .GetMembers("OnAfterExecuteAsync")
            .OfType<IMethodSymbol>()
            .Any(method => method.OverriddenMethod is not null);
    }

    private static ITypeSymbol? GetExpectedResultType(
        ReplacementContext replacement,
        ReplacementType replacementType)
    {
        return replacementType switch
        {
            ReplacementType.List => replacement.SemanticModel.Compilation
                .GetTypeByMetadataName("System.Collections.Generic.List`1")?
                .Construct(replacement.ElementTypeSymbol),
            ReplacementType.Array => replacement.SemanticModel.Compilation
                .CreateArrayTypeSymbol(replacement.ElementTypeSymbol),
            _ => null,
        };
    }

    private static ITypeSymbol GetExecutionResultType(ITypeSymbol returnType)
    {
        return returnType is INamedTypeSymbol { Name: "Task", TypeArguments.Length: 1 } taskType
            ? taskType.TypeArguments[0]
            : returnType;
    }

    private static int? GetModuleResultTypeParameterOrdinal(
        INamedTypeSymbol baseType,
        INamedTypeSymbol moduleType)
    {
        var baseTypeDefinition = baseType.OriginalDefinition;
        for (var current = baseTypeDefinition; current is not null; current = current.BaseType)
        {
            if (current.IsGenericType
                && SymbolEqualityComparer.Default.Equals(current.OriginalDefinition, moduleType)
                && current.TypeArguments[0] is ITypeParameterSymbol typeParameter
                && SymbolEqualityComparer.Default.Equals(
                    typeParameter.ContainingType,
                    baseTypeDefinition))
            {
                return typeParameter.Ordinal;
            }
        }

        return null;
    }

    private sealed class ReplacementContext(
        Document document,
        SyntaxNode documentRoot,
        SemanticModel semanticModel,
        TypeSyntax enumerableType,
        GenericNameSyntax enumerableGenericType,
        TypeSyntax elementType,
        ITypeSymbol elementTypeSymbol)
    {
        public Document Document { get; } = document;

        public SyntaxNode DocumentRoot { get; } = documentRoot;

        public SemanticModel SemanticModel { get; } = semanticModel;

        public TypeSyntax EnumerableType { get; } = enumerableType;

        public GenericNameSyntax EnumerableGenericType { get; } = enumerableGenericType;

        public TypeSyntax ElementType { get; } = elementType;

        public ITypeSymbol ElementTypeSymbol { get; } = elementTypeSymbol;
    }

    private enum ReplacementType
    {
        List,
        Array,
    }
}
