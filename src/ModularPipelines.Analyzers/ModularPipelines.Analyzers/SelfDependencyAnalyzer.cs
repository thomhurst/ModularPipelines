using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Analyzer that detects when a module depends on itself via DependsOnAttribute.
/// Reports diagnostic MPDEP003 when a module has [DependsOn&lt;Self&gt;].
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class SelfDependencyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MPDEP003";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.SelfDependencyAnalyzerTitle),
        nameof(Resources.SelfDependencyAnalyzerMessageFormat),
        nameof(Resources.SelfDependencyAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
    }

    private void AnalyzeAttribute(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AttributeSyntax attributeSyntax)
        {
            return;
        }

        var attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;

        if (attributeSymbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        var namedTypeSymbol = methodSymbol.ContainingType;

        if (!IsDependsOnAttributeType(namedTypeSymbol, context.Compilation))
        {
            return;
        }

        // Get the type argument from the attribute
        ITypeSymbol? dependencyType = null;

        // Check for generic attribute: [DependsOn<SomeType>]
        if (namedTypeSymbol.IsGenericType && namedTypeSymbol.TypeArguments.Length > 0)
        {
            dependencyType = namedTypeSymbol.TypeArguments[0];
        }

        // Check for non-generic attribute: [DependsOn(typeof(SomeType))]
        else if (attributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.Expression is TypeOfExpressionSyntax typeOfExpression)
        {
            var typeInfo = context.SemanticModel.GetTypeInfo(typeOfExpression.Type);
            dependencyType = typeInfo.Type;
        }

        if (dependencyType is null)
        {
            return;
        }

        // Get the containing type (the module class that has this attribute)
        var containingType = context.GetClassThatNodeIsIn();
        if (containingType is null)
        {
            return;
        }

        // Check if the dependency type is the same as the containing type (self-dependency)
        if (SymbolEqualityComparer.Default.Equals(dependencyType, containingType))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                Rule,
                attributeSyntax.GetLocation(),
                containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static bool IsDependsOnAttributeType(INamedTypeSymbol typeSymbol, Compilation compilation)
    {
        var dependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute");
        if (dependsOnAttributeType is null)
        {
            return false;
        }

        var typeToCompare = typeSymbol.IsGenericType
            ? typeSymbol.OriginalDefinition
            : typeSymbol;

        if (SymbolEqualityComparer.Default.Equals(typeToCompare, dependsOnAttributeType))
        {
            return true;
        }

        var genericDependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute`1");
        return genericDependsOnAttributeType is not null &&
               SymbolEqualityComparer.Default.Equals(typeToCompare, genericDependsOnAttributeType);
    }
}
