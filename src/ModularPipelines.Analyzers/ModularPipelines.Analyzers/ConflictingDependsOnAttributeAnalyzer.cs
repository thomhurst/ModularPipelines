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
    
    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ConflictingDependsOnAttributeAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ConflictingDependsOnAttributeAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ConflictingDependsOnAttributeAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

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
        if (context.Node is not AttributeSyntax attributeSyntax)
        {
            return;
        }
        
        if (attributeSyntax.Name is not GenericNameSyntax genericNameSyntax)
        {
            return;
        }

        if (genericNameSyntax.Identifier.ValueText is not "DependsOn")
        {
            return;
        }
        
        var attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;

        if (attributeSymbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        var namedTypeSymbol = methodSymbol.ContainingType;
        
        if (!namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::ModularPipelines.Attributes.DependsOnAttribute"))
        {
            return;
        }

        if (!namedTypeSymbol.IsGenericType ||
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

        foreach (var conflictingDependencyAttribute in allAttributesOnDependentType.Where(x => x.IsDependsOnAttributeFor(typeContainingAttribute)))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), namedArgumentTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), conflictingDependencyAttribute.AttributeClass?.TypeArguments.First().ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }
}
