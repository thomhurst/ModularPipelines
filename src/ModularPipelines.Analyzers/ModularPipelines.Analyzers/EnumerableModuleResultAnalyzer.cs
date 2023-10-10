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

    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

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

        if (simpleBaseTypeSyntax.Type is not GenericNameSyntax genericNameSyntax)
        {
            return;
        }

        if (genericNameSyntax.Identifier.ValueText is not "Module")
        {
            return;
        }

        if (genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault() is not GenericNameSyntax
            innerGenericNameSyntax)
        {
            return;
        }

        if (innerGenericNameSyntax.Identifier.ValueText is not "IEnumerable")
        {
            return;
        }

        var genericArgumentSymbol = context.SemanticModel.GetSymbolInfo(innerGenericNameSyntax).Symbol;

        if (genericArgumentSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return;
        }

        if (genericArgumentSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::System.Collections.Generic.IEnumerable<"))
        {
            var properties = new Dictionary<string, string?>
            {
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }
}
