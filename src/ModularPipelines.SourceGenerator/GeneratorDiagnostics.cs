using Microsoft.CodeAnalysis;

namespace ModularPipelines.SourceGenerator;

internal static class GeneratorDiagnostics
{
    private const string Category = "ModularPipelines.SourceGenerator";
    private const string HelpLinkBase =
        "https://thomhurst.github.io/ModularPipelines/docs/how-to/source-generator-diagnostics";

    public static DiagnosticDescriptor InvalidIntegrationMethod { get; } = Create(
        "MPG0001",
        "Invalid Modular Pipelines integration registrar",
        "Method '{0}' marked with [ModularPipelinesIntegration] must be an accessible, "
        + "non-generic static method on an accessible, non-generic type; it must accept exactly "
        + "one by-value IServiceCollection parameter and return void or IServiceCollection",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor DuplicateModuleAccessor { get; } = Create(
        "MPG0002",
        "Duplicate generated module accessor",
        "Generated module accessor '{0}' conflicts for module types: {1}",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor IncompleteCommandMetadata { get; } = Create(
        "MPG0003",
        "Incomplete command metadata",
        "Command metadata for '{0}' is incomplete because one or more attributed properties "
        + "are inaccessible; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor IncompleteSecretMetadata { get; } = Create(
        "MPG0004",
        "Incomplete secret metadata",
        "Secret metadata for '{0}' is incomplete because one or more [SecretValue] properties "
        + "are inaccessible; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor IncompleteModuleEventMetadata { get; } = Create(
        "MPG0005",
        "Incomplete module event metadata",
        "Module event metadata for '{0}' is incomplete because one or more attributes cannot "
        + "be constructed by generated code; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor SkippedRuntimeMetadata { get; } = Create(
        "MPG0006",
        "Runtime metadata generation skipped",
        "Runtime metadata generation for '{0}' was skipped because the type is generic or "
        + "inaccessible; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor SkippedModuleEventMetadata { get; } = Create(
        "MPG0007",
        "Module event metadata generation skipped",
        "Module event metadata generation for '{0}' was skipped because the concrete module "
        + "type is generic or inaccessible; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    private static DiagnosticDescriptor Create(
        string id,
        string title,
        string messageFormat,
        DiagnosticSeverity severity)
    {
        return new DiagnosticDescriptor(
            id,
            title,
            messageFormat,
            Category,
            severity,
            isEnabledByDefault: true,
            helpLinkUri: $"{HelpLinkBase}#{id.ToLowerInvariant()}");
    }
}
