using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class EnumerableModuleResultAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "EnumerableModuleResult";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.IEnumerableModuleResultAnalyzerTitle),
        nameof(Resources.IEnumerableModuleResultAnalyzerMessageFormat),
        nameof(Resources.IEnumerableModuleResultAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeIEnumerableModuleResults, SyntaxKind.SimpleBaseType);
    }

    private void AnalyzeIEnumerableModuleResults(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not SimpleBaseTypeSyntax simpleBaseTypeSyntax)
        {
            return;
        }

        if (context.SemanticModel.GetTypeInfo(
                simpleBaseTypeSyntax.Type,
                context.CancellationToken).Type is not INamedTypeSymbol baseType)
        {
            return;
        }

        var moduleType = context.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.Module);
        var enumerableType = context.Compilation.GetTypeByMetadataName(
            AnalyzerConstants.FullyQualifiedTypeNames.IEnumerable);

        if (moduleType is null || enumerableType is null)
        {
            return;
        }

        var moduleResultType = GetModuleResultType(baseType, moduleType);
        if (moduleResultType is not INamedTypeSymbol namedTypeSymbol
            || !SymbolEqualityComparer.Default.Equals(
                namedTypeSymbol.OriginalDefinition,
                enumerableType))
        {
            return;
        }

        var properties = new Dictionary<string, string?>
        {
            ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
        }.ToImmutableDictionary();

        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            context.Node.GetLocation(),
            properties,
            namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }

    private static ITypeSymbol? GetModuleResultType(
        INamedTypeSymbol baseType,
        INamedTypeSymbol moduleType)
    {
        for (var current = baseType; current is not null; current = current.BaseType)
        {
            if (current.IsGenericType
                && SymbolEqualityComparer.Default.Equals(
                    current.OriginalDefinition,
                    moduleType))
            {
                return current.TypeArguments[0];
            }
        }

        return null;
    }
}
