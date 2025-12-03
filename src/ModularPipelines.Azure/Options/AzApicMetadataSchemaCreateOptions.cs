using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "metadata-schema", "create")]
public record AzApicMetadataSchemaCreateOptions(
[property: CliOption("--metadata-schema")] string MetadataSchema,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--assigned-to")]
    public string? AssignedTo { get; set; }

    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }
}