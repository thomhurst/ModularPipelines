using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "add-metadata")]
public record GcloudComputeInstancesAddMetadataOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}