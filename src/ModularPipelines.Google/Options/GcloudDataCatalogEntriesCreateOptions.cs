using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "entries", "create")]
public record GcloudDataCatalogEntriesCreateOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location,
[property: CliOption("--type")] string Type,
[property: CliOption("--gcs-file-patterns")] string[] GcsFilePatterns,
[property: CliOption("--user-specified-system")] string UserSpecifiedSystem,
[property: CliOption("--user-specified-type")] string UserSpecifiedType,
[property: CliOption("--linked-resource")] string LinkedResource,
[property: CliOption("--source-system-create-time")] string SourceSystemCreateTime,
[property: CliOption("--source-system-update-time")] string SourceSystemUpdateTime
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--fully-qualified-name")]
    public string? FullyQualifiedName { get; set; }

    [CliOption("--schema")]
    public string[]? Schema { get; set; }

    [CliOption("--schema-from-file")]
    public string? SchemaFromFile { get; set; }
}