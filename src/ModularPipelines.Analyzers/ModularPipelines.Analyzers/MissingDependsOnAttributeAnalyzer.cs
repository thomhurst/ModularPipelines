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
public class MissingDependsOnAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MissingDependsOnAttribute";
    
    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MissingDependsOnAttributeAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.MissingDependsOnAttributeAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.MissingDependsOnAttributeAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeMissingDependsOnAttributes, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeMissingDependsOnAttributes(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return;
        }

        if (invocationExpressionSyntax.Expression is not GenericNameSyntax genericNameSyntax)
        {
            return;
        }

        if (genericNameSyntax.Identifier.ValueText is not "GetModule")
        {
            return;
        }

        var genericArgument = genericNameSyntax.TypeArgumentList.Arguments.First();

        var genericArgumentSymbol = context.SemanticModel.GetSymbolInfo(genericArgument).Symbol;

        if (genericArgumentSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return;
        }

        var classContainingGetModuleCallSyntax = GetClassDeclarationSyntax(invocationExpressionSyntax);

        if (classContainingGetModuleCallSyntax is null)
        {
            return;
        }

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classContainingGetModuleCallSyntax);

        if (classSymbol is null)
        {
            return;
        }

        var attributes = classSymbol.GetAllAttributesIncludingBaseAndInterfaces();

        if (!attributes.Any(x => x.IsDependsOnAttributeFor(namedTypeSymbol)))
        {
            var properties = new Dictionary<string, string?>
            {
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private ClassDeclarationSyntax? GetClassDeclarationSyntax(InvocationExpressionSyntax invocationExpressionSyntax)
    {
        var parent = invocationExpressionSyntax.Parent;

        while (parent is not null)
        {
            if (parent is ClassDeclarationSyntax classDeclarationSyntax)
            {
                return classDeclarationSyntax;
            }

            parent = parent.Parent;
        }

        return null;
    }
}
