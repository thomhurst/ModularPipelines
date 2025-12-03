using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "rolling-action", "replace")]
public record GcloudComputeInstanceGroupsManagedRollingActionReplaceOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--max-surge")]
    public string? MaxSurge { get; set; }

    [CliOption("--max-unavailable")]
    public string? MaxUnavailable { get; set; }

    [CliOption("--replacement-method")]
    public string? ReplacementMethod { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}