using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EnumerableModuleResultAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "EnumerableModuleResult";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.IEnumerableModuleResultAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Usage";

    public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }
}
