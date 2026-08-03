using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>Detects colliding CLI switches.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class CliOptionCollisionAnalyzer : DiagnosticAnalyzer
{
    public const string DuplicateSwitchDiagnosticId = "MPCLI004";
    public static DiagnosticDescriptor DuplicateSwitchRule { get; } = DiagnosticDescriptorFactory.Create(
        DuplicateSwitchDiagnosticId, "DuplicateCliSwitchTitle", "DuplicateCliSwitchMessageFormat", "DuplicateCliSwitchDescription");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [DuplicateSwitchRule];

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
        if (type.TypeKind != TypeKind.Class || !type.InheritsFrom(symbols.CommandLineToolOptions))
        {
            return;
        }

        var switches = new Dictionary<string, IPropertySymbol>(StringComparer.Ordinal);

        foreach (var property in GetEffectivePropertiesBaseFirst(type))
        {
            var attribute = FindCommandAttribute(property, symbols);
            if (attribute is not null
                && (CliAttributeSymbols.Is(attribute, symbols.CliFlag)
                    || CliAttributeSymbols.Is(attribute, symbols.CliOption)))
            {
                AnalyzeSwitch(context, type, property, attribute, switches);
            }
        }
    }

    private static AttributeData? FindCommandAttribute(
        IPropertySymbol property,
        CliAttributeSymbols symbols)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            var attribute = current.GetAttributes().FirstOrDefault(candidate =>
                CliAttributeSymbols.Is(candidate, symbols.CliArgument)
                || CliAttributeSymbols.Is(candidate, symbols.CliFlag)
                || CliAttributeSymbols.Is(candidate, symbols.CliOption));
            if (attribute is not null)
            {
                return attribute;
            }
        }

        return null;
    }

    private static IEnumerable<IPropertySymbol> GetEffectivePropertiesBaseFirst(INamedTypeSymbol type)
    {
        var propertiesByType = new List<IReadOnlyList<IPropertySymbol>>();
        var seenNames = new HashSet<string>(StringComparer.Ordinal);
        for (var current = type; current is not null; current = current.BaseType)
        {
            var properties = current.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(property => !property.IsStatic
                                   && property.GetMethod is not null
                                   && seenNames.Add(property.Name))
                .ToList();
            propertiesByType.Add(properties);
        }

        propertiesByType.Reverse();
        return propertiesByType.SelectMany(properties => properties);
    }

    private static void AnalyzeSwitch(
        SymbolAnalysisContext context,
        INamedTypeSymbol analyzedType,
        IPropertySymbol property,
        AttributeData attribute,
        Dictionary<string, IPropertySymbol> switches)
    {
        foreach (var switchName in GetSwitchNames(attribute))
        {
            if (switches.TryGetValue(switchName, out var existingProperty))
            {
                if (SymbolEqualityComparer.Default.Equals(property.ContainingType, analyzedType)
                    && !SymbolEqualityComparer.Default.Equals(property, existingProperty)
                    && !Overrides(property, existingProperty))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        DuplicateSwitchRule,
                        CliAttributeSymbols.GetLocation(attribute, property),
                        switchName,
                        GetQualifiedName(existingProperty),
                        GetQualifiedName(property)));
                }
            }
            else
            {
                switches.Add(switchName, property);
            }
        }
    }

    private static IEnumerable<string> GetSwitchNames(AttributeData attribute)
    {
        var name = attribute.ConstructorArguments.FirstOrDefault().Value as string;
        if (name is not null)
        {
            yield return name;
        }

        var shortForm = attribute.NamedArguments.FirstOrDefault(pair => pair.Key == "ShortForm").Value.Value as string;
        if (!string.IsNullOrWhiteSpace(shortForm) && !string.Equals(name, shortForm, StringComparison.Ordinal))
        {
            yield return shortForm!;
        }
    }

    private static string GetQualifiedName(IPropertySymbol property) =>
        $"{property.ContainingType.Name}.{property.Name}";

    private static bool Overrides(IPropertySymbol property, IPropertySymbol other)
    {
        for (var overridden = property.OverriddenProperty; overridden is not null; overridden = overridden.OverriddenProperty)
        {
            if (SymbolEqualityComparer.Default.Equals(overridden, other))
            {
                return true;
            }
        }

        return false;
    }
}
