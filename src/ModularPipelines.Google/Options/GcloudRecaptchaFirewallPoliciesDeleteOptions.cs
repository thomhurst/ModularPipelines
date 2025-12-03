using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "firewall-policies", "delete")]
public record GcloudRecaptchaFirewallPoliciesDeleteOptions(
[property: CliArgument] string FirewallPolicy
) : GcloudOptions;