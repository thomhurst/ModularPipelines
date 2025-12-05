using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "firewall-policies", "describe")]
public record GcloudRecaptchaFirewallPoliciesDescribeOptions(
[property: CliArgument] string FirewallPolicy
) : GcloudOptions;