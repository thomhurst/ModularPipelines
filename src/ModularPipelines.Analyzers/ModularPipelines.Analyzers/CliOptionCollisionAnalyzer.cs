using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

/// <summary>Detects colliding CLI switches and positional arguments.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class CliOptionCollisionAnalyzer : DiagnosticAnalyzer
{
    private const int DefaultArgumentPhase = 3;
    private const int DefaultArgumentPlacement = 0;

    public const string DuplicateSwitchDiagnosticId = "MPCLI004";
    public const string DuplicateArgumentPositionDiagnosticId = "MPCLI005";

    public static DiagnosticDescriptor DuplicateSwitchRule { get; } = DiagnosticDescriptorFactory.Create(
        DuplicateSwitchDiagnosticId, "DuplicateCliSwitchTitle", "DuplicateCliSwitchMessageFormat", "DuplicateCliSwitchDescription");

    public static DiagnosticDescriptor DuplicateArgumentPositionRule { get; } = DiagnosticDescriptorFactory.Create(
        DuplicateArgumentPositionDiagnosticId, "DuplicateCliArgumentPositionTitle", "DuplicateCliArgumentPositionMessageFormat", "DuplicateCliArgumentPositionDescription");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DuplicateSwitchRule, DuplicateArgumentPositionRule);

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
        var positions = new Dictionary<(int Position, int Phase, int Placement), IPropertySymbol>();

        foreach (var property in GetPropertiesBaseFirst(type))
        {
            foreach (var attribute in property.GetAttributes())
            {
                if (CliAttributeSymbols.Is(attribute, symbols.CliFlag)
                    || CliAttributeSymbols.Is(attribute, symbols.CliOption))
                {
                    AnalyzeSwitch(context, type, property, attribute, switches);
                }
                else if (CliAttributeSymbols.Is(attribute, symbols.CliArgument))
                {
                    AnalyzeArgument(context, type, property, attribute, positions);
                }
            }
        }
    }

    private static IEnumerable<IPropertySymbol> GetPropertiesBaseFirst(INamedTypeSymbol type)
    {
        var hierarchy = new Stack<INamedTypeSymbol>();
        for (var current = type; current is not null; current = current.BaseType)
        {
            hierarchy.Push(current);
        }

        while (hierarchy.Count > 0)
        {
            foreach (var property in hierarchy.Pop().GetMembers().OfType<IPropertySymbol>())
            {
                yield return property;
            }
        }
    }

    private static void AnalyzeSwitch(
        SymbolAnalysisContext context,
        INamedTypeSymbol analyzedType,
        IPropertySymbol property,
        AttributeData attribute,
        IDictionary<string, IPropertySymbol> switches)
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

    private static void AnalyzeArgument(
        SymbolAnalysisContext context,
        INamedTypeSymbol analyzedType,
        IPropertySymbol property,
        AttributeData attribute,
        IDictionary<(int Position, int Phase, int Placement), IPropertySymbol> positions)
    {
        var position = attribute.ConstructorArguments.FirstOrDefault().Value as int? ?? 0;
        var phase = GetNamedEnumValue(attribute, "Phase") ?? DefaultArgumentPhase;
        var placement = GetNamedEnumValue(attribute, "Placement") ?? DefaultArgumentPlacement;
        var orderedPhase = placement == DefaultArgumentPlacement ? phase : -1;
        var key = (position, orderedPhase, placement);

        if (positions.TryGetValue(key, out var existingProperty))
        {
            if (SymbolEqualityComparer.Default.Equals(property.ContainingType, analyzedType)
                && !Overrides(property, existingProperty))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DuplicateArgumentPositionRule,
                    CliAttributeSymbols.GetLocation(attribute, property),
                    position,
                    GetQualifiedName(existingProperty),
                    GetQualifiedName(property)));
            }
        }
        else
        {
            positions.Add(key, property);
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

    private static int? GetNamedEnumValue(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(pair => pair.Key == name).Value.Value as int?;

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
