using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "remove-metadata")]
public record GcloudComputeInstancesRemoveMetadataOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--keys")]
    public string[]? Keys { get; set; }
}