using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "update-from-file")]
public record GcloudComputeInstancesUpdateFromFileOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--minimal-action")]
    public string? MinimalAction { get; set; }

    [CliOption("--most-disruptive-allowed-action")]
    public string? MostDisruptiveAllowedAction { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}