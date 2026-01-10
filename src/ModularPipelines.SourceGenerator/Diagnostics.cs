using Microsoft.CodeAnalysis;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Diagnostic descriptors for the CommandOptionsGenerator.
/// </summary>
internal static class Diagnostics
{
    /// <summary>
    /// Error: Options class must be marked 'partial' for source generation.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingPartialKeyword = new(
        id: "MP0001",
        title: "Options class must be partial",
        messageFormat: "Options class '{0}' must be marked 'partial' for source generation",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    /// <summary>
    /// Warning: Options class has no [CliTool] attribute in its inheritance chain.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingCliToolAttribute = new(
        id: "MP0002",
        title: "Missing [CliTool] attribute",
        messageFormat: "Options class '{0}' has no [CliTool] attribute in its inheritance chain",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    /// <summary>
    /// Error: Multiple properties have the same [CliArgument] position.
    /// </summary>
    public static readonly DiagnosticDescriptor DuplicateArgumentPosition = new(
        id: "MP0003",
        title: "Duplicate argument position",
        messageFormat: "Properties {0} have duplicate [CliArgument] at position {1}",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    /// <summary>
    /// Info: Property has no CLI attribute and will be ignored during command building.
    /// Note: This diagnostic will be reported when GetCliProperties is fully implemented
    /// to scan properties and detect those without CLI attributes.
    /// </summary>
    public static readonly DiagnosticDescriptor UnmarkedProperty = new(
        id: "MP0004",
        title: "Property has no CLI attribute",
        messageFormat: "Property '{0}' has no CLI attribute and will be ignored",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);
}
