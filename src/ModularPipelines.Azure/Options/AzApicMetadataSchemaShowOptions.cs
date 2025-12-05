using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "metadata-schema", "show")]
public record AzApicMetadataSchemaShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--metadata-schema")]
    public string? MetadataSchema { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}