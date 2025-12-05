using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "update")]
public record GcloudComputeSecurityPoliciesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-layer7-ddos-defense")]
    public bool? EnableLayer7DdosDefense { get; set; }

    [CliOption("--json-custom-content-types")]
    public string[]? JsonCustomContentTypes { get; set; }

    [CliOption("--json-parsing")]
    public string? JsonParsing { get; set; }

    [CliOption("--layer7-ddos-defense-rule-visibility")]
    public string? Layer7DdosDefenseRuleVisibility { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--network-ddos-protection")]
    public string? NetworkDdosProtection { get; set; }

    [CliOption("--recaptcha-redirect-site-key")]
    public string? RecaptchaRedirectSiteKey { get; set; }

    [CliOption("--user-ip-request-headers")]
    public string[]? UserIpRequestHeaders { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}