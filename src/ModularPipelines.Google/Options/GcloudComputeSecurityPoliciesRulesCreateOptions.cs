using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "rules", "create")]
public record GcloudComputeSecurityPoliciesRulesCreateOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--expression")] string Expression,
[property: CommandSwitch("--network-dest-ip-ranges")] string[] NetworkDestIpRanges,
[property: CommandSwitch("--network-dest-ports")] string[] NetworkDestPorts,
[property: CommandSwitch("--network-ip-protocols")] string[] NetworkIpProtocols,
[property: CommandSwitch("--network-src-asns")] string[] NetworkSrcAsns,
[property: CommandSwitch("--network-src-ip-ranges")] string[] NetworkSrcIpRanges,
[property: CommandSwitch("--network-src-ports")] string[] NetworkSrcPorts,
[property: CommandSwitch("--network-src-region-codes")] string[] NetworkSrcRegionCodes,
[property: CommandSwitch("--network-user-defined-fields")] string[] NetworkUserDefinedFields,
[property: CommandSwitch("--src-ip-ranges")] string[] SrcIpRanges
) : GcloudOptions
{
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
}