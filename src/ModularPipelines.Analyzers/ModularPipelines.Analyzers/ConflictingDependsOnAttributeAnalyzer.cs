using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class ConflictingDependsOnAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ConflictingDependsOnAttribute";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerTitle),
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerMessageFormat),
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeConflictingDependsOnAttributes, SyntaxKind.Attribute);
    }

    private void AnalyzeConflictingDependsOnAttributes(SyntaxNodeAnalysisContext context)
    {
        if (!IsDependsOn(context, out var namedTypeSymbol))
        {
            return;
        }

        if (!namedTypeSymbol!.IsGenericType ||
            namedTypeSymbol.TypeArguments.FirstOrDefault() is not INamedTypeSymbol namedArgumentTypeSymbol)
        {
            return;
        }

        var typeContainingAttribute = context.GetClassThatNodeIsIn();

        if (typeContainingAttribute is null)
        {
            return;
        }

        var allAttributesOnDependentType = namedArgumentTypeSymbol.GetAllAttributesIncludingBaseAndInterfaces();

        ReportDiagnostics(context, allAttributesOnDependentType, typeContainingAttribute, namedArgumentTypeSymbol);
    }

    private static bool IsDependsOn(SyntaxNodeAnalysisContext context, out INamedTypeSymbol? namedTypeSymbol)
    {
        namedTypeSymbol = null;

        if (context.Node is not AttributeSyntax attributeSyntax)
        {
            return false;
        }

        if (attributeSyntax.Name is not GenericNameSyntax genericNameSyntax)
        {
            return false;
        }

        if (genericNameSyntax.Identifier.ValueText is not "DependsOn")
        {
            return false;
        }

        var attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;

        if (attributeSymbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        namedTypeSymbol = methodSymbol.ContainingType;

        if (!IsDependsOnAttributeType(namedTypeSymbol, context.Compilation))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the given type symbol is the DependsOnAttribute type (generic or non-generic).
    /// Uses proper symbol comparison instead of string comparison.
    /// </summary>
    private static bool IsDependsOnAttributeType(INamedTypeSymbol typeSymbol, Compilation compilation)
    {
        // Get the non-generic DependsOnAttribute type
        var dependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute");
        if (dependsOnAttributeType is null)
        {
            return false;
        }

        // For generic types, compare the original definition
        var typeToCompare = typeSymbol.IsGenericType
            ? typeSymbol.OriginalDefinition
            : typeSymbol;

        // Check if it's the non-generic version
        if (SymbolEqualityComparer.Default.Equals(typeToCompare, dependsOnAttributeType))
        {
            return true;
        }

        // Get and check the generic version (DependsOnAttribute`1)
        var genericDependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute`1");
        return genericDependsOnAttributeType is not null &&
               SymbolEqualityComparer.Default.Equals(typeToCompare, genericDependsOnAttributeType);
    }

    private static void ReportDiagnostics(SyntaxNodeAnalysisContext context, IEnumerable<AttributeData> allAttributesOnDependentType,
        INamedTypeSymbol typeContainingAttribute, INamedTypeSymbol namedArgumentTypeSymbol)
    {
        foreach (var conflictingDependencyAttribute in allAttributesOnDependentType.Where(x =>
                     x.IsDependsOnAttributeFor(context.Compilation, typeContainingAttribute)))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(),
                namedArgumentTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                conflictingDependencyAttribute.AttributeClass?.TypeArguments.First()
                    .ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }
}