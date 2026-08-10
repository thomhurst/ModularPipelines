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
        "Command metadata for '{0}' is incomplete because these attributed properties are "
        + "inaccessible or have invalid or conflicting command attributes: {1}; make every "
        + "attributed property accessible and declare only one command attribute per property",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor IncompleteSecretMetadata { get; } = Create(
        "MPG0004",
        "Incomplete secret metadata",
        "Secret metadata for '{0}' is incomplete because these [SecretValue] properties are "
        + "inaccessible: {1}; make every [SecretValue] property accessible",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor IncompleteModuleEventMetadata { get; } = Create(
        "MPG0005",
        "Incomplete module event metadata",
        "Module event metadata for '{0}' is incomplete because one or more attributes cannot "
        + "be constructed by generated code; runtime reflection will be used",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor SkippedRuntimeMetadata { get; } = Create(
        "MPG0006",
        "Runtime metadata generation skipped",
        "Runtime metadata generation for '{0}' was skipped because the type is generic, "
        + "inaccessible, file-local, split across partial declarations, or has an ambiguous "
        + "metadata name across assemblies; make the type and its containing types accessible, "
        + "non-generic, and non-file-local, avoid split partial declarations in its hierarchy, "
        + "and use unique metadata names",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor SkippedModuleEventMetadata { get; } = Create(
        "MPG0007",
        "Module event metadata generation skipped",
        "Module event metadata generation for '{0}' was skipped because the concrete module "
        + "type is generic or inaccessible; runtime reflection will be used",
        DiagnosticSeverity.Info);

    public static DiagnosticDescriptor UnsupportedToolsLanguageVersion { get; } = Create(
        "MPG0008",
        "Discoverable tool properties require C# 14",
        "Tool accessor '{0}' cannot generate a context.Tools property because language version "
        + "'{1}' does not support extension members; use C# 14 or preview, or call the "
        + "compatibility accessor context.{2}()",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor ConflictingToolProperty { get; } = Create(
        "MPG0009",
        "Conflicting discoverable tool property",
        "Tool property '{0}' has conflicting declarations: {1}",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor ShadowedToolProperty { get; } = Create(
        "MPG0010",
        "Discoverable tool property shadows an instance member",
        "Tool accessor '{0}' cannot generate property '{1}' because that name is already "
        + "available on IToolsContext or object",
        DiagnosticSeverity.Error);

    public static DiagnosticDescriptor SkippedModuleRuntimeMetadata { get; } = Create(
        "MPG0011",
        "Module runtime metadata generation skipped",
        "Runtime metadata generation for module '{0}' was skipped because the type is "
        + "inaccessible to generated code; make the module and its containing types accessible "
        + "before publishing with Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor ExternalClosedGenericModuleRuntimeMetadata { get; } = Create(
        "MPG0012",
        "External closed generic module lacks runtime metadata",
        "Runtime metadata for externally declared closed generic module '{0}' cannot be generated "
        + "from this consumer usage; use a consumer-owned non-generic wrapper before publishing "
        + "with Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor GenericModuleRegistrationRuntimeMetadata { get; } = Create(
        "MPG0013",
        "Generic module registration lacks runtime metadata",
        "Runtime metadata cannot be generated for AddModule<{0}> because the module type is a "
        + "type parameter; register each concrete module type directly before publishing with "
        + "Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor PartialModuleRuntimeMetadata { get; } = Create(
        "MPG0014",
        "Partial module dependency metadata is incomplete",
        "Runtime dependency metadata for partial module '{0}' is incomplete because another "
        + "source generator can contribute partial declarations; avoid partial module "
        + "declarations before publishing with Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor NonConcreteModuleRegistrationRuntimeMetadata { get; } = Create(
        "MPG0015",
        "Non-concrete module registration lacks runtime metadata",
        "Runtime metadata cannot be generated for AddModule because the static module type "
        + "'{0}' is not concrete; register the concrete module type directly before publishing "
        + "with Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor SelectorDependencyRuntimeMetadata { get; } = Create(
        "MPG0016",
        "Dependency metadata requires reflection",
        "Dependency metadata for module '{0}' requires runtime attribute reflection and may "
        + "be removed by trimming; use built-in explicit DependsOn<T> dependencies before "
        + "publishing with Native AOT",
        DiagnosticSeverity.Warning);

    public static DiagnosticDescriptor PeerGeneratedRuntimeMetadata { get; } = Create(
        "MPG0017",
        "Peer-generated type lacks runtime metadata",
        "Runtime metadata for '{0}' cannot be generated because the type was emitted by another "
        + "source generator in this compilation; declare the type in user source or a referenced "
        + "assembly so it is visible to the metadata generator",
        DiagnosticSeverity.Error);

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
