using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-instances", "update")]
public record GcloudComputeTargetInstancesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}