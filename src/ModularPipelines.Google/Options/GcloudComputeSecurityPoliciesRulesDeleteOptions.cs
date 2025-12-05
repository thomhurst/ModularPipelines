using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "rules", "delete")]
public record GcloudComputeSecurityPoliciesRulesDeleteOptions(
[property: CliArgument] string Priority
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }
}