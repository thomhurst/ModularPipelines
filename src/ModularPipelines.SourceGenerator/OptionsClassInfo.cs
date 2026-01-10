using Microsoft.CodeAnalysis;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Information about a CommandLineToolOptions class discovered by the generator.
/// </summary>
internal sealed record OptionsClassInfo(
    string Namespace,
    string ClassName,
    string? ToolName,
    IReadOnlyList<string> SubCommandParts,
    IReadOnlyList<CliPropertyInfo> Properties,
    bool IsPartial,
    Location Location
);

/// <summary>
/// Information about a CLI property on an options class.
/// </summary>
internal sealed record CliPropertyInfo(
    string Name,
    string TypeName,
    CliPropertyKind Kind,
    string? CliName,
    string? ShortForm,
    int? ArgumentPosition,
    bool AllowMultiple
);

/// <summary>
/// The kind of CLI property.
/// </summary>
internal enum CliPropertyKind
{
    Option,
    Flag,
    Argument,
    Unknown,
}
