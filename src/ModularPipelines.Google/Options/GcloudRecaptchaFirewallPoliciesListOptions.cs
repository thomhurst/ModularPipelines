using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "firewall-policies", "list")]
public record GcloudRecaptchaFirewallPoliciesListOptions : GcloudOptions;