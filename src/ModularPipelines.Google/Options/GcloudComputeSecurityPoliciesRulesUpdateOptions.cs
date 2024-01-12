using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "rules", "update")]
public record GcloudComputeSecurityPoliciesRulesUpdateOptions(
[property: PositionalArgument] string Priority
) : GcloudOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--ban-duration-sec")]
    public string? BanDurationSec { get; set; }

    [CommandSwitch("--ban-threshold-count")]
    public string? BanThresholdCount { get; set; }

    [CommandSwitch("--ban-threshold-interval-sec")]
    public string? BanThresholdIntervalSec { get; set; }

    [CommandSwitch("--conform-action")]
    public string? ConformAction { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enforce-on-key")]
    public string? EnforceOnKey { get; set; }

    [CommandSwitch("--enforce-on-key-configs")]
    public string[]? EnforceOnKeyConfigs { get; set; }

    [CommandSwitch("--enforce-on-key-name")]
    public string? EnforceOnKeyName { get; set; }

    [CommandSwitch("--exceed-action")]
    public string? ExceedAction { get; set; }

    [CommandSwitch("--exceed-redirect-target")]
    public string? ExceedRedirectTarget { get; set; }

    [CommandSwitch("--exceed-redirect-type")]
    public string? ExceedRedirectType { get; set; }

    [BooleanCommandSwitch("--preview")]
    public bool? Preview { get; set; }

    [CommandSwitch("--rate-limit-threshold-count")]
    public string? RateLimitThresholdCount { get; set; }

    [CommandSwitch("--rate-limit-threshold-interval-sec")]
    public string? RateLimitThresholdIntervalSec { get; set; }

    [CommandSwitch("--recaptcha-action-site-keys")]
    public string[]? RecaptchaActionSiteKeys { get; set; }

    [CommandSwitch("--recaptcha-session-site-keys")]
    public string[]? RecaptchaSessionSiteKeys { get; set; }

    [CommandSwitch("--redirect-target")]
    public string? RedirectTarget { get; set; }

    [CommandSwitch("--redirect-type")]
    public string? RedirectType { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--request-headers-to-add")]
    public string[]? RequestHeadersToAdd { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--expression")]
    public string? Expression { get; set; }

    [CommandSwitch("--network-dest-ip-ranges")]
    public string[]? NetworkDestIpRanges { get; set; }

    [CommandSwitch("--network-dest-ports")]
    public string[]? NetworkDestPorts { get; set; }

    [CommandSwitch("--network-ip-protocols")]
    public string[]? NetworkIpProtocols { get; set; }

    [CommandSwitch("--network-src-asns")]
    public string[]? NetworkSrcAsns { get; set; }

    [CommandSwitch("--network-src-ip-ranges")]
    public string[]? NetworkSrcIpRanges { get; set; }

    [CommandSwitch("--network-src-ports")]
    public string[]? NetworkSrcPorts { get; set; }

    [CommandSwitch("--network-src-region-codes")]
    public string[]? NetworkSrcRegionCodes { get; set; }

    [CommandSwitch("--network-user-defined-fields")]
    public string[]? NetworkUserDefinedFields { get; set; }

    [CommandSwitch("--src-ip-ranges")]
    public string[]? SrcIpRanges { get; set; }
}