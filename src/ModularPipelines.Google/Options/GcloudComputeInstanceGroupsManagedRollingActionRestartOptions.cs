using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "rolling-action", "restart")]
public record GcloudComputeInstanceGroupsManagedRollingActionRestartOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--max-unavailable")]
    public string? MaxUnavailable { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}