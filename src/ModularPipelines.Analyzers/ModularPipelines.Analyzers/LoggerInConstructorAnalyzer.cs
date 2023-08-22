using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class LoggerInConstructorAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "LoggerInConstructor";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Usage";

    public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeLoggersInConstructors, SyntaxKind.ConstructorDeclaration);
    }

    private void AnalyzeLoggersInConstructors(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            return;
        }
        
        foreach (var parameter in constructorDeclarationSyntax.ParameterList.Parameters)
        {
            if (IsGenericILogger(context, parameter, out var parameterSymbol))
            {
                ReportDiagnostic(context, parameter.GetLocation(), parameterSymbol!);
                return;
            }

            foreach (var injectedLoggerType in ImmutableArray.Create("ILogger", "ILoggerProvider", "ILoggerFactory"))
            {
                if (IsNonGenericType(context, parameter, injectedLoggerType, out parameterSymbol))
                {
                    ReportDiagnostic(context, parameter.GetLocation(), parameterSymbol!);
                    return;
                }
            }
        }
    }
    
    private static bool IsGenericILogger(SyntaxNodeAnalysisContext context, ParameterSyntax parameter, out INamedTypeSymbol? parameterSymbol)
    {
        parameterSymbol = null;
        
        if (parameter.Type is not GenericNameSyntax genericNameSyntax)
        {
            return false;
        }
        
        if (genericNameSyntax.Identifier.ValueText is not "ILogger")
        {
            return false;
        }
        
        var genericArgumentSymbol = context.SemanticModel.GetSymbolInfo(genericNameSyntax).Symbol;

        if (genericArgumentSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return false;
        }

        if (!genericArgumentSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Microsoft.Extensions.Logging.ILogger<"))
        {
            return false;
        }

        parameterSymbol = namedTypeSymbol;
        return true;
    }

    private static bool IsNonGenericType(SyntaxNodeAnalysisContext context, ParameterSyntax parameter, string name, out INamedTypeSymbol? parameterSymbol)
    {
        parameterSymbol = null;
        
        if (parameter.Type is not IdentifierNameSyntax identifierNameSyntax)
        {
            return false;
        }
        
        if (identifierNameSyntax.Identifier.ValueText != name)
        {
            return false;
        }
        
        var genericArgumentSymbol = context.SemanticModel.GetSymbolInfo(identifierNameSyntax).Symbol;

        if (genericArgumentSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return false;
        }

        if (!genericArgumentSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith($"global::Microsoft.Extensions.Logging.{name}"))
        {
            return false;
        }

        parameterSymbol = namedTypeSymbol;
        return true;
    }

    private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, Location location, INamedTypeSymbol namedTypeSymbol)
    {
        var properties = new Dictionary<string, string?>
        {
            ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
        }.ToImmutableDictionary();

        context.ReportDiagnostic(Diagnostic.Create(Rule, location, properties,
            namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }
}
