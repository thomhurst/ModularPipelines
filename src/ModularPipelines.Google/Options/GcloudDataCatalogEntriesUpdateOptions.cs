using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entries", "update")]
public record GcloudDataCatalogEntriesUpdateOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--lookup-entry")]
    public string? LookupEntry { get; set; }

    [CliOption("--add-file-patterns")]
    public string[]? AddFilePatterns { get; set; }

    [CliFlag("--clear-file-patterns")]
    public bool? ClearFilePatterns { get; set; }

    [CliOption("--remove-file-patterns")]
    public string[]? RemoveFilePatterns { get; set; }

    [CliOption("--linked-resource")]
    public string? LinkedResource { get; set; }

    [CliOption("--user-specified-system")]
    public string? UserSpecifiedSystem { get; set; }

    [CliOption("--user-specified-type")]
    public string? UserSpecifiedType { get; set; }

    [CliOption("--schema")]
    public string[]? Schema { get; set; }

    [CliOption("--schema-from-file")]
    public string? SchemaFromFile { get; set; }
}