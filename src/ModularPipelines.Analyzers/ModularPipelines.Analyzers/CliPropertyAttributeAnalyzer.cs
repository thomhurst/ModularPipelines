using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers;

/// <summary>Validates CLI attributes applied to option properties.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class CliPropertyAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string InvalidFlagTypeDiagnosticId = "MPCLI001";
    public const string BooleanOptionDiagnosticId = "MPCLI002";
    public const string MultipleAttributesDiagnosticId = "MPCLI003";
    public const string NegatedFlagTypeDiagnosticId = "MPCLI005";

    public static DiagnosticDescriptor InvalidFlagTypeRule { get; } = DiagnosticDescriptorFactory.Create(
        InvalidFlagTypeDiagnosticId, "CliFlagInvalidTypeTitle", "CliFlagInvalidTypeMessageFormat", "CliFlagInvalidTypeDescription");

    public static DiagnosticDescriptor BooleanOptionRule { get; } = DiagnosticDescriptorFactory.Create(
        BooleanOptionDiagnosticId, "CliBooleanOptionTitle", "CliBooleanOptionMessageFormat", "CliBooleanOptionDescription");

    public static DiagnosticDescriptor MultipleAttributesRule { get; } = DiagnosticDescriptorFactory.Create(
        MultipleAttributesDiagnosticId, "MultipleCliAttributesTitle", "MultipleCliAttributesMessageFormat", "MultipleCliAttributesDescription");

    public static DiagnosticDescriptor NegatedFlagTypeRule { get; } = DiagnosticDescriptorFactory.Create(
        NegatedFlagTypeDiagnosticId, "NegatedFlagInvalidTypeTitle", "NegatedFlagInvalidTypeMessageFormat", "NegatedFlagInvalidTypeDescription");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(InvalidFlagTypeRule, BooleanOptionRule, MultipleAttributesRule, NegatedFlagTypeRule);

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
                    symbolContext => AnalyzeProperty(symbolContext, symbols),
                    SymbolKind.Property);
            }
        });
    }

    private static void AnalyzeProperty(SymbolAnalysisContext context, CliAttributeSymbols symbols)
    {
        var property = (IPropertySymbol) context.Symbol;
        var cliAttributes = property.GetAttributes().Where(symbols.IsCliPropertyAttribute).ToImmutableArray();

        if (cliAttributes.Length > 1)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                MultipleAttributesRule,
                CliAttributeSymbols.GetLocation(cliAttributes[1], property),
                property.Name));
        }

        foreach (var attribute in cliAttributes)
        {
            var isFlag = CliAttributeSymbols.Is(attribute, symbols.CliFlag);
            if (isFlag
                && HasNamedStringValue(attribute, "NegatedName")
                && !IsNullableBoolean(property.Type))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    NegatedFlagTypeRule,
                    CliAttributeSymbols.GetLocation(attribute, property),
                    property.Name,
                    property.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
            }
            else if (isFlag && !IsBooleanOrInteger(property.Type))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    InvalidFlagTypeRule,
                    CliAttributeSymbols.GetLocation(attribute, property),
                    property.Name,
                    property.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
            }

            if (CliAttributeSymbols.Is(attribute, symbols.CliOption)
                && IsNullableBoolean(property.Type)
                && symbols.CliOptionValueArityNone is not null
                && GetNamedEnumValue(attribute, "ValueArity") == symbols.CliOptionValueArityNone)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    BooleanOptionRule,
                    CliAttributeSymbols.GetLocation(attribute, property),
                    property.Name));
            }
        }
    }

    private static bool IsBooleanOrInteger(ITypeSymbol type) =>
        type.SpecialType is SpecialType.System_Boolean or SpecialType.System_Int32
        || IsNullableOf(type, SpecialType.System_Boolean)
        || IsNullableOf(type, SpecialType.System_Int32);

    private static bool IsNullableBoolean(ITypeSymbol type) => IsNullableOf(type, SpecialType.System_Boolean);

    private static bool IsNullableOf(ITypeSymbol type, SpecialType underlyingType) =>
        type is INamedTypeSymbol namedType
        && namedType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
        && namedType.TypeArguments.Length == 1
        && namedType.TypeArguments[0].SpecialType == underlyingType;

    private static int? GetNamedEnumValue(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(pair => pair.Key == name).Value.Value as int?;

    private static bool HasNamedStringValue(AttributeData attribute, string name) =>
        attribute.NamedArguments.Any(pair => pair.Key == name && pair.Value.Value is string);
}
