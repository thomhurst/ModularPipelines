using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recaptcha", "firewall-policies", "delete")]
public record GcloudRecaptchaFirewallPoliciesDeleteOptions(
[property: PositionalArgument] string FirewallPolicy
) : GcloudOptions;