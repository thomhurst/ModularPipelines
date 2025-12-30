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
public class LoggerInConstructorAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "LoggerInConstructor";

    public static DiagnosticDescriptor Rule => PrivateRule;

    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.LoggerInConstructorAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private static readonly DiagnosticDescriptor PrivateRule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    /// <summary>
    /// Logging types from Microsoft.Extensions.Logging that should not be injected directly.
    /// </summary>
    private static readonly ImmutableArray<string> LoggingTypeMetadataNames = ImmutableArray.Create(
        "Microsoft.Extensions.Logging.ILogger`1",
        "Microsoft.Extensions.Logging.ILogger",
        "Microsoft.Extensions.Logging.ILoggerProvider",
        "Microsoft.Extensions.Logging.ILoggerFactory");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeLoggersInConstructors, SyntaxKind.ConstructorDeclaration);
    }

    private static void AnalyzeLoggersInConstructors(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            return;
        }

        foreach (var parameter in constructorDeclarationSyntax.ParameterList.Parameters)
        {
            if (TryGetProhibitedLoggerType(context, parameter, out var parameterSymbol))
            {
                ReportDiagnostic(context, parameter.GetLocation(), parameterSymbol!);
                return;
            }
        }
    }

    /// <summary>
    /// Checks if the parameter type is a prohibited logging type from Microsoft.Extensions.Logging.
    /// </summary>
    private static bool TryGetProhibitedLoggerType(
        SyntaxNodeAnalysisContext context,
        ParameterSyntax parameter,
        out INamedTypeSymbol? parameterSymbol)
    {
        parameterSymbol = null;

        if (parameter.Type is null)
        {
            return false;
        }

        var typeSymbol = context.SemanticModel.GetSymbolInfo(parameter.Type).Symbol;
        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return false;
        }

        if (!namedTypeSymbol.IsAnyType(context.Compilation, LoggingTypeMetadataNames.AsSpan()))
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
            ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
        }.ToImmutableDictionary();

        context.ReportDiagnostic(Diagnostic.Create(Rule, location, properties,
            namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }
}