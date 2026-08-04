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
/// Reports diagnostic MP0010 when a module has [DependsOn&lt;Self&gt;].
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class SelfDependencyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0010";

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

        context.RegisterCompilationStartAction(startContext =>
        {
            var usingAliasNames = startContext.Compilation.GetUsingAliasNames(
                startContext.CancellationToken);
            startContext.RegisterSyntaxNodeAction(
                syntaxContext => AnalyzeAttribute(syntaxContext, usingAliasNames),
                SyntaxKind.Attribute);
        });
    }

    private void AnalyzeAttribute(
        SyntaxNodeAnalysisContext context,
        ImmutableHashSet<string> usingAliasNames)
    {
        if (!TryGetDependsOnInfo(
                context,
                usingAliasNames,
                out var attributeSyntax,
                out var attributeType,
                out var dependencyType))
        {
            return;
        }

        var containingType = context.GetClassThatNodeIsIn();
        if (containingType is null)
        {
            return;
        }

        if (IsSelfDependency(dependencyType, containingType))
        {
            ReportSelfDependency(context, attributeSyntax, containingType);
        }
    }

    private static bool TryGetDependsOnInfo(
        SyntaxNodeAnalysisContext context,
        ImmutableHashSet<string> usingAliasNames,
        out AttributeSyntax attributeSyntax,
        out INamedTypeSymbol attributeType,
        out ITypeSymbol dependencyType)
    {
        attributeSyntax = null!;
        attributeType = null!;
        dependencyType = null!;

        if (context.Node is not AttributeSyntax attrSyntax
            || !attrSyntax.CouldBeDependsOnAttribute(usingAliasNames))
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

    private static bool IsSelfDependency(ITypeSymbol dependencyType, INamedTypeSymbol containingType)
    {
        return SymbolEqualityComparer.Default.Equals(dependencyType, containingType);
    }

    private static void ReportSelfDependency(
        SyntaxNodeAnalysisContext context,
        AttributeSyntax attributeSyntax,
        INamedTypeSymbol containingType)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            attributeSyntax.GetLocation(),
            containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }
}
