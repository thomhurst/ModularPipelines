using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "firewall-policies", "describe")]
public record GcloudRecaptchaFirewallPoliciesDescribeOptions(
[property: PositionalArgument] string FirewallPolicy
) : GcloudOptions;