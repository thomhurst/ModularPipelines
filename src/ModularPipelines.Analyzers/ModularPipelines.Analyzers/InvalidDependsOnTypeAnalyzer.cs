using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Analyzer that detects when DependsOnAttribute references a type that doesn't implement IModule.
/// Reports diagnostic MPDEP001 when DependsOn references a non-module type.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class InvalidDependsOnTypeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MPDEP001";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.InvalidDependsOnTypeAnalyzerTitle),
        nameof(Resources.InvalidDependsOnTypeAnalyzerMessageFormat),
        nameof(Resources.InvalidDependsOnTypeAnalyzerDescription));

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
        if (!TryGetDependsOnInfo(context, out var attributeSyntax, out var attributeType, out var dependencyType))
        {
            return;
        }

        if (!IsValidModuleType(dependencyType, context.Compilation))
        {
            ReportInvalidDependsOnType(context, attributeSyntax, dependencyType);
        }
    }

    private static bool TryGetDependsOnInfo(
        SyntaxNodeAnalysisContext context,
        out AttributeSyntax attributeSyntax,
        out INamedTypeSymbol attributeType,
        out ITypeSymbol dependencyType)
    {
        attributeSyntax = null!;
        attributeType = null!;
        dependencyType = null!;

        if (context.Node is not AttributeSyntax attrSyntax)
        {
            return false;
        }

        attributeSyntax = attrSyntax;

        if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        attributeType = methodSymbol.ContainingType;

        if (!attributeType.IsDependsOnAttribute(context.Compilation))
        {
            return false;
        }

        var depType = attributeType.GetDependsOnTypeArgument(attributeSyntax, context.SemanticModel);
        if (depType is null)
        {
            return false;
        }

        dependencyType = depType;
        return true;
    }

    private static bool IsValidModuleType(ITypeSymbol dependencyType, Compilation compilation)
    {
        var iModuleType = compilation.GetTypeByMetadataName("ModularPipelines.Modules.IModule");
        return iModuleType is not null && dependencyType.ImplementsInterface(iModuleType);
    }

    private static void ReportInvalidDependsOnType(
        SyntaxNodeAnalysisContext context,
        AttributeSyntax attributeSyntax,
        ITypeSymbol dependencyType)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            attributeSyntax.GetLocation(),
            dependencyType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }
}
