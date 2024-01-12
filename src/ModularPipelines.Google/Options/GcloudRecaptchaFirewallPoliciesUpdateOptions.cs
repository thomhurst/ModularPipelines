using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "firewall-policies", "update")]
public record GcloudRecaptchaFirewallPoliciesUpdateOptions(
[property: PositionalArgument] string FirewallPolicy
) : GcloudOptions
{
    [CommandSwitch("--actions")]
    public string? Actions { get; set; }

    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}