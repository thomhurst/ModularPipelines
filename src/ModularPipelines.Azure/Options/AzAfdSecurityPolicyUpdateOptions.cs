using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "security-policy", "update")]
public record AzAfdSecurityPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--domains")]
    public string? Domains { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--security-policy-name")]
    public string? SecurityPolicyName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }
}