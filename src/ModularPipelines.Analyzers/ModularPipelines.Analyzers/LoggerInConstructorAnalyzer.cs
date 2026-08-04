using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class LoggerInConstructorAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0003";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.LoggerInConstructorAnalyzerTitle),
        nameof(Resources.LoggerInConstructorAnalyzerMessageFormat),
        nameof(Resources.LoggerInConstructorAnalyzerDescription));

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

        context.RegisterSymbolAction(AnalyzeLoggersInConstructors, SymbolKind.NamedType);
    }

    private static void AnalyzeLoggersInConstructors(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol namedType
            || !namedType.IsModule(context.Compilation))
        {
            return;
        }

        foreach (var parameter in namedType.InstanceConstructors
                     .SelectMany(static constructor => constructor.Parameters))
        {
            if (TryGetProhibitedLoggerType(context, parameter, out var parameterSymbol)
                && GetParameterLocation(parameter, context.CancellationToken) is { } location)
            {
                ReportDiagnostic(context, location, parameterSymbol);
            }
        }
    }

    /// <summary>
    /// Checks if the parameter type is a prohibited logging type from Microsoft.Extensions.Logging.
    /// </summary>
    private static bool TryGetProhibitedLoggerType(
        SymbolAnalysisContext context,
        IParameterSymbol parameter,
        out INamedTypeSymbol parameterSymbol)
    {
        if (parameter.Type is not INamedTypeSymbol namedTypeSymbol
            || !namedTypeSymbol.IsAnyType(
                context.Compilation,
                LoggingTypeMetadataNames.AsSpan()))
        {
            parameterSymbol = null!;
            return false;
        }

        parameterSymbol = namedTypeSymbol;
        return true;
    }

    private static Location? GetParameterLocation(
        IParameterSymbol parameter,
        CancellationToken cancellationToken)
    {
        return parameter.DeclaringSyntaxReferences
            .Select(reference => reference.GetSyntax(cancellationToken))
            .OfType<ParameterSyntax>()
            .Select(static syntax => syntax.GetLocation())
            .FirstOrDefault();
    }

    private static void ReportDiagnostic(
        SymbolAnalysisContext context,
        Location location,
        INamedTypeSymbol namedTypeSymbol)
    {
        var properties = new Dictionary<string, string?>
        {
            ["Name"] = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
        }.ToImmutableDictionary();

        context.ReportDiagnostic(Diagnostic.Create(Rule, location, properties,
            namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }
}
