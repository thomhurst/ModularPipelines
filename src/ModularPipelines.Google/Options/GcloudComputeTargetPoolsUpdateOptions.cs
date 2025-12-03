using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "update")]
public record GcloudComputeTargetPoolsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }
}