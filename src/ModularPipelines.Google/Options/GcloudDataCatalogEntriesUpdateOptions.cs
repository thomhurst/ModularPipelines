using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "entries", "update")]
public record GcloudDataCatalogEntriesUpdateOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--lookup-entry")]
    public string? LookupEntry { get; set; }

    [CommandSwitch("--add-file-patterns")]
    public string[]? AddFilePatterns { get; set; }

    [BooleanCommandSwitch("--clear-file-patterns")]
    public bool? ClearFilePatterns { get; set; }

    [CommandSwitch("--remove-file-patterns")]
    public string[]? RemoveFilePatterns { get; set; }

    [CommandSwitch("--linked-resource")]
    public string? LinkedResource { get; set; }

    [CommandSwitch("--user-specified-system")]
    public string? UserSpecifiedSystem { get; set; }

    [CommandSwitch("--user-specified-type")]
    public string? UserSpecifiedType { get; set; }

    [CommandSwitch("--schema")]
    public string[]? Schema { get; set; }

    [CommandSwitch("--schema-from-file")]
    public string? SchemaFromFile { get; set; }
}