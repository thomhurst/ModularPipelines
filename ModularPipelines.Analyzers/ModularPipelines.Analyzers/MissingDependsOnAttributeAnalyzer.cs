using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MissingDependsOnAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MissingDependsOnAttribute";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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

        var attributes = GetAttributes(classSymbol);

        if (!attributes.Any(x => IsDependsOnAttributeFor(x, namedTypeSymbol)))
        {
            var properties = new Dictionary<string, string?>
            {
                ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
            }.ToImmutableDictionary();

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), properties, namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private IEnumerable<AttributeData> GetAttributes(INamedTypeSymbol classSymbol)
    {
        foreach (var attributeData in classSymbol.AllInterfaces.SelectMany(x => x.GetAttributes()))
        {
            yield return attributeData;
        }

        while (true)
        {
            foreach (var attributeData in classSymbol.GetAttributes())
            {
                yield return attributeData;
            }

            var baseType = classSymbol.BaseType;

            if (baseType == null)
            {
                yield break;
            }

            classSymbol = baseType;
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

    private bool IsDependsOnAttributeFor(AttributeData attributeData, INamedTypeSymbol namedTypeSymbol)
    {
        var attributeClassName = attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (string.IsNullOrEmpty(attributeClassName))
        {
            return false;
        }
        
        if (!attributeClassName!.StartsWith("global::ModularPipelines.Attributes.DependsOnAttribute"))
        {
            return false;
        }

        if (attributeData.AttributeClass!.IsGenericType)
        {
            return SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass.TypeArguments.First(), namedTypeSymbol);
        }

        return attributeData.ConstructorArguments.Any(x =>
        {
            var argumentValue = x.Value;

            if (argumentValue is INamedTypeSymbol argumentNamedTypeSymbol)
            {
                return SymbolEqualityComparer.Default.Equals(argumentNamedTypeSymbol, namedTypeSymbol);
            }

            return false;
        });
    }
}