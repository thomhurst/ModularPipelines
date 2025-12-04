using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "security-policy", "show")]
public record AzAfdSecurityPolicyShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--security-policy-name")]
    public string? SecurityPolicyName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}