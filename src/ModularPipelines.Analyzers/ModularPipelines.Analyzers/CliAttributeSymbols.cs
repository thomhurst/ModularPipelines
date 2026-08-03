using Microsoft.CodeAnalysis;

namespace ModularPipelines.Analyzers;

internal sealed class CliAttributeSymbols
{
    private const string AttributesNamespace = "ModularPipelines.Attributes.";

    private CliAttributeSymbols(Compilation compilation)
    {
        CommandLineToolOptions = compilation.GetTypeByMetadataName("ModularPipelines.Options.CommandLineToolOptions");
        CliArgument = compilation.GetTypeByMetadataName(AttributesNamespace + "CliArgumentAttribute");
        CliFlag = compilation.GetTypeByMetadataName(AttributesNamespace + "CliFlagAttribute");
        CliOption = compilation.GetTypeByMetadataName(AttributesNamespace + "CliOptionAttribute");
        CliSubCommand = compilation.GetTypeByMetadataName(AttributesNamespace + "CliSubCommandAttribute");
        CliTool = compilation.GetTypeByMetadataName(AttributesNamespace + "CliToolAttribute");
        CommandLinePhasePassthrough = GetEnumConstantValue(compilation, "CommandLinePhase", "Passthrough");
        ArgumentPlacementAfterOptions = GetEnumConstantValue(compilation, "ArgumentPlacement", "AfterOptions");
        CliOptionValueArityNone = GetEnumConstantValue(compilation, "CliOptionValueArity", "None");
    }

    public INamedTypeSymbol? CommandLineToolOptions { get; }
    public INamedTypeSymbol? CliArgument { get; }
    public INamedTypeSymbol? CliFlag { get; }
    public INamedTypeSymbol? CliOption { get; }
    public INamedTypeSymbol? CliSubCommand { get; }
    public INamedTypeSymbol? CliTool { get; }
    public int? CommandLinePhasePassthrough { get; }
    public int? ArgumentPlacementAfterOptions { get; }
    public int? CliOptionValueArityNone { get; }

    public bool IsAvailable => CommandLineToolOptions is not null
                               && CliArgument is not null
                               && CliFlag is not null
                               && CliOption is not null
                               && CliSubCommand is not null
                               && CliTool is not null;

    public static CliAttributeSymbols Create(Compilation compilation) => new(compilation);

    public bool IsCliPropertyAttribute(AttributeData attribute) =>
        Is(attribute, CliArgument) || Is(attribute, CliFlag) || Is(attribute, CliOption);

    public bool IsCliTypeAttribute(AttributeData attribute) =>
        Is(attribute, CliSubCommand) || Is(attribute, CliTool);

    public static bool Is(AttributeData attribute, INamedTypeSymbol? attributeType) =>
        SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeType);

    public static Location GetLocation(AttributeData attribute, ISymbol fallbackSymbol) =>
        attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()
        ?? fallbackSymbol.Locations.FirstOrDefault()
        ?? Location.None;

    private static int? GetEnumConstantValue(Compilation compilation, string enumName, string memberName) =>
        compilation.GetTypeByMetadataName(AttributesNamespace + enumName)?
            .GetMembers(memberName)
            .OfType<IFieldSymbol>()
            .FirstOrDefault(field => field.HasConstantValue)?
            .ConstantValue as int?;
}
