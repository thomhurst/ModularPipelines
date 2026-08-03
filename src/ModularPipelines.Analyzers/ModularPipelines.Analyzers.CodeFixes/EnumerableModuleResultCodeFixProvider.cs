using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumerableModuleResultCodeFixProvider))]
[Shared]
[ExcludeFromCodeCoverage]
public class EnumerableModuleResultCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(EnumerableModuleResultAnalyzer.DiagnosticId);

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

        if (HasCompatibleExecutionOverride(replacement, ReplacementType.List))
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.EnumerableModuleResultToListCodeFixTitle,
                    createChangedDocument: c => ReplaceEnumerableWithList(context, baseTypeSyntax, c),
                    equivalenceKey: nameof(CodeFixResources.EnumerableModuleResultToListCodeFixTitle)),
                diagnostic);
        }

        if (HasCompatibleExecutionOverride(replacement, ReplacementType.Array))
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

        var listType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SingletonSeparatedList(replacement.ElementType)));
        var newRoot = replacement.DocumentRoot.ReplaceNode(
            replacement.EnumerableType,
            listType.WithTriviaFrom(replacement.EnumerableType));

        newRoot = AddUsingIfNeeded(newRoot, "System.Collections.Generic");

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
            arrayType.WithTriviaFrom(replacement.EnumerableType)));
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

        var moduleType = semanticModel.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.Module);
        var resultArgumentOrdinal = moduleType is null
            ? null
            : GetModuleResultTypeParameterOrdinal(baseTypeSymbol, moduleType);
        var baseGenericName = baseType.Type
            .DescendantNodesAndSelf()
            .OfType<GenericNameSyntax>()
            .FirstOrDefault(candidate => SymbolEqualityComparer.Default.Equals(
                semanticModel.GetTypeInfo(candidate, cancellationToken).Type,
                baseTypeSymbol));
        if (resultArgumentOrdinal is not int ordinal
            || baseGenericName is null
            || ordinal >= baseGenericName.TypeArgumentList.Arguments.Count)
        {
            return null;
        }

        var candidate = baseGenericName.TypeArgumentList.Arguments[ordinal];
        if (semanticModel.GetTypeInfo(candidate, cancellationToken).Type
                is not INamedTypeSymbol candidateType
            || !SymbolEqualityComparer.Default.Equals(
                candidateType.OriginalDefinition,
                enumerableType))
        {
            return null;
        }

        var enumerableGenericName = candidate
            .DescendantNodesAndSelf()
            .OfType<GenericNameSyntax>()
            .FirstOrDefault(generic => semanticModel.GetTypeInfo(generic, cancellationToken).Type
                    is INamedTypeSymbol genericType
                && SymbolEqualityComparer.Default.Equals(
                    genericType.OriginalDefinition,
                    enumerableType));
        var elementType = enumerableGenericName?.TypeArgumentList.Arguments.FirstOrDefault();
        var elementTypeSymbol = elementType is null
            ? null
            : semanticModel.GetTypeInfo(elementType, cancellationToken).Type;
        return elementType is null || elementTypeSymbol is null
            ? null
            : new ReplacementContext(
                documentRoot,
                semanticModel,
                candidate,
                elementType,
                elementTypeSymbol);
    }

    private static bool HasCompatibleExecutionOverride(
        ReplacementContext replacement,
        ReplacementType replacementType)
    {
        var containingType = replacement.EnumerableType
            .Ancestors()
            .OfType<TypeDeclarationSyntax>()
            .FirstOrDefault();
        if (containingType is null
            || replacement.SemanticModel.GetDeclaredSymbol(containingType)
                is not INamedTypeSymbol containingTypeSymbol)
        {
            return false;
        }

        var executionOverrides = containingTypeSymbol
            .GetMembers()
            .OfType<IMethodSymbol>()
            .Where(method => method.OverriddenMethod is not null
                && method.Name is "Execute" or "ExecuteAsync")
            .ToArray();
        if (executionOverrides.Length == 0)
        {
            return true;
        }

        ITypeSymbol? expectedResultType = replacementType switch
        {
            ReplacementType.List => replacement.SemanticModel.Compilation
                .GetTypeByMetadataName("System.Collections.Generic.List`1")?
                .Construct(replacement.ElementTypeSymbol),
            ReplacementType.Array => replacement.SemanticModel.Compilation
                .CreateArrayTypeSymbol(replacement.ElementTypeSymbol),
            _ => null,
        };
        return expectedResultType is not null
            && executionOverrides.All(method => SymbolEqualityComparer.Default.Equals(
                GetExecutionResultType(method.ReturnType),
                expectedResultType));
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

    private static SyntaxNode AddUsingIfNeeded(SyntaxNode documentRoot, string namespaceName)
    {
        if (documentRoot is not CompilationUnitSyntax compilationUnitSyntax)
        {
            return documentRoot;
        }

        if (compilationUnitSyntax.Usings.Any(u => u.Name?.ToFullString() == namespaceName))
        {
            return documentRoot;
        }

        return compilationUnitSyntax.AddUsings(
            SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName)));
    }

    private sealed class ReplacementContext(
        SyntaxNode documentRoot,
        SemanticModel semanticModel,
        TypeSyntax enumerableType,
        TypeSyntax elementType,
        ITypeSymbol elementTypeSymbol)
    {
        public SyntaxNode DocumentRoot { get; } = documentRoot;

        public SemanticModel SemanticModel { get; } = semanticModel;

        public TypeSyntax EnumerableType { get; } = enumerableType;

        public TypeSyntax ElementType { get; } = elementType;

        public ITypeSymbol ElementTypeSymbol { get; } = elementTypeSymbol;
    }

    private enum ReplacementType
    {
        List,
        Array,
    }
}
