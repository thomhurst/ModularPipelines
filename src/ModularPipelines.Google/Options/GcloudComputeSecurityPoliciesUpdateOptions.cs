using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "update")]
public record GcloudComputeSecurityPoliciesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-layer7-ddos-defense")]
    public bool? EnableLayer7DdosDefense { get; set; }

    [CommandSwitch("--json-custom-content-types")]
    public string[]? JsonCustomContentTypes { get; set; }

    [CommandSwitch("--json-parsing")]
    public string? JsonParsing { get; set; }

    [CommandSwitch("--layer7-ddos-defense-rule-visibility")]
    public string? Layer7DdosDefenseRuleVisibility { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--network-ddos-protection")]
    public string? NetworkDdosProtection { get; set; }

    [CommandSwitch("--recaptcha-redirect-site-key")]
    public string? RecaptchaRedirectSiteKey { get; set; }

    [CommandSwitch("--user-ip-request-headers")]
    public string[]? UserIpRequestHeaders { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}