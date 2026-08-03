using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>Validates the type hierarchy used by CLI attributes.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class CliOptionsIdentityAnalyzer : DiagnosticAnalyzer
{
    public const string InvalidOptionsBaseDiagnosticId = "MPCLI006";

    public static DiagnosticDescriptor InvalidOptionsBaseRule { get; } = DiagnosticDescriptorFactory.Create(
        InvalidOptionsBaseDiagnosticId, "CliOptionsBaseTitle", "CliOptionsBaseMessageFormat", "CliOptionsBaseDescription");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [InvalidOptionsBaseRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext =>
        {
            var symbols = CliAttributeSymbols.Create(startContext.Compilation);
            if (symbols.IsAvailable)
            {
                startContext.RegisterSymbolAction(
                    symbolContext => AnalyzeType(symbolContext, symbols),
                    SymbolKind.NamedType);
            }
        });
    }

    private static void AnalyzeType(SymbolAnalysisContext context, CliAttributeSymbols symbols)
    {
        var type = (INamedTypeSymbol) context.Symbol;
        var ownTypeAttributes = type.GetAttributes().Where(symbols.IsCliTypeAttribute).ToImmutableArray();
        var ownCliProperties = type.GetMembers().OfType<IPropertySymbol>()
            .Where(property => property.GetAttributes().Any(symbols.IsCliPropertyAttribute))
            .ToImmutableArray();

        if (ownTypeAttributes.Length == 0 && ownCliProperties.Length == 0)
        {
            return;
        }

        if (!type.InheritsFrom(symbols.CommandLineToolOptions))
        {
            var location = ownTypeAttributes.FirstOrDefault() is { } attribute
                ? CliAttributeSymbols.GetLocation(attribute, type)
                : type.Locations.FirstOrDefault() ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(InvalidOptionsBaseRule, location, type.Name));
        }
    }
}
