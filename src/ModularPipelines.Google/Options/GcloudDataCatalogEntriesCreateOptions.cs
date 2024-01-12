using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "create")]
public record GcloudDataCatalogEntriesCreateOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--gcs-file-patterns")] string[] GcsFilePatterns,
[property: CommandSwitch("--user-specified-system")] string UserSpecifiedSystem,
[property: CommandSwitch("--user-specified-type")] string UserSpecifiedType,
[property: CommandSwitch("--linked-resource")] string LinkedResource,
[property: CommandSwitch("--source-system-create-time")] string SourceSystemCreateTime,
[property: CommandSwitch("--source-system-update-time")] string SourceSystemUpdateTime
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--fully-qualified-name")]
    public string? FullyQualifiedName { get; set; }

    [CommandSwitch("--schema")]
    public string[]? Schema { get; set; }

    [CommandSwitch("--schema-from-file")]
    public string? SchemaFromFile { get; set; }
}