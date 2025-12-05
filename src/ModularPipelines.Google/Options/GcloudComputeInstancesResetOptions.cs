using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "reset")]
public record GcloudComputeInstancesResetOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}