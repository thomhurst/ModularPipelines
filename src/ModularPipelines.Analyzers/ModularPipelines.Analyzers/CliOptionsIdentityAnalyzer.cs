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
    public const string ConflictingToolDiagnosticId = "MPCLI007";
    public const string MissingToolDiagnosticId = "MPCLI008";

    public static DiagnosticDescriptor InvalidOptionsBaseRule { get; } = DiagnosticDescriptorFactory.Create(
        InvalidOptionsBaseDiagnosticId, "CliOptionsBaseTitle", "CliOptionsBaseMessageFormat", "CliOptionsBaseDescription");

    public static DiagnosticDescriptor ConflictingToolRule { get; } = DiagnosticDescriptorFactory.Create(
        ConflictingToolDiagnosticId, "ConflictingCliToolTitle", "ConflictingCliToolMessageFormat", "ConflictingCliToolDescription");

    public static DiagnosticDescriptor MissingToolRule { get; } = DiagnosticDescriptorFactory.Create(
        MissingToolDiagnosticId, "MissingCliToolTitle", "MissingCliToolMessageFormat", "MissingCliToolDescription",
        severity: DiagnosticSeverity.Warning);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(InvalidOptionsBaseRule, ConflictingToolRule, MissingToolRule);

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
        if (type.TypeKind != TypeKind.Class)
        {
            return;
        }

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
            return;
        }

        AnalyzeToolIdentity(context, type, symbols, ownTypeAttributes);
    }

    private static void AnalyzeToolIdentity(
        SymbolAnalysisContext context,
        INamedTypeSymbol type,
        CliAttributeSymbols symbols,
        ImmutableArray<AttributeData> ownTypeAttributes)
    {
        var ownTool = ownTypeAttributes.FirstOrDefault(attribute => CliAttributeSymbols.Is(attribute, symbols.CliTool));
        var inheritedTool = FindInheritedAttribute(type.BaseType, symbols.CliTool);

        var hasOwnSubCommand = ownTypeAttributes.Any(attribute => CliAttributeSymbols.Is(attribute, symbols.CliSubCommand));
        if (hasOwnSubCommand && ownTool is null && inheritedTool is null)
        {
            var subCommand = ownTypeAttributes.First(attribute => CliAttributeSymbols.Is(attribute, symbols.CliSubCommand));
            context.ReportDiagnostic(Diagnostic.Create(
                MissingToolRule,
                CliAttributeSymbols.GetLocation(subCommand, type),
                type.Name));
        }
    }

    private static AttributeData? FindInheritedAttribute(INamedTypeSymbol? type, INamedTypeSymbol? attributeType)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            var attribute = current.GetAttributes().FirstOrDefault(item => CliAttributeSymbols.Is(item, attributeType));
            if (attribute is not null)
            {
                return attribute;
            }
        }

        return null;
    }
}
