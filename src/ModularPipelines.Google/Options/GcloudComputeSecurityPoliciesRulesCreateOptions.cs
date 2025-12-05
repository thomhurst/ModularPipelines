using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "rules", "create")]
public record GcloudComputeSecurityPoliciesRulesCreateOptions(
[property: CliArgument] string Priority,
[property: CliOption("--action")] string Action,
[property: CliOption("--expression")] string Expression,
[property: CliOption("--network-dest-ip-ranges")] string[] NetworkDestIpRanges,
[property: CliOption("--network-dest-ports")] string[] NetworkDestPorts,
[property: CliOption("--network-ip-protocols")] string[] NetworkIpProtocols,
[property: CliOption("--network-src-asns")] string[] NetworkSrcAsns,
[property: CliOption("--network-src-ip-ranges")] string[] NetworkSrcIpRanges,
[property: CliOption("--network-src-ports")] string[] NetworkSrcPorts,
[property: CliOption("--network-src-region-codes")] string[] NetworkSrcRegionCodes,
[property: CliOption("--network-user-defined-fields")] string[] NetworkUserDefinedFields,
[property: CliOption("--src-ip-ranges")] string[] SrcIpRanges
) : GcloudOptions
{
    [CliOption("--ban-duration-sec")]
    public string? BanDurationSec { get; set; }

    [CliOption("--ban-threshold-count")]
    public string? BanThresholdCount { get; set; }

    [CliOption("--ban-threshold-interval-sec")]
    public string? BanThresholdIntervalSec { get; set; }

    [CliOption("--conform-action")]
    public string? ConformAction { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enforce-on-key")]
    public string? EnforceOnKey { get; set; }

    [CliOption("--enforce-on-key-configs")]
    public string[]? EnforceOnKeyConfigs { get; set; }

    [CliOption("--enforce-on-key-name")]
    public string? EnforceOnKeyName { get; set; }

    [CliOption("--exceed-action")]
    public string? ExceedAction { get; set; }

    [CliOption("--exceed-redirect-target")]
    public string? ExceedRedirectTarget { get; set; }

    [CliOption("--exceed-redirect-type")]
    public string? ExceedRedirectType { get; set; }

    [CliFlag("--preview")]
    public bool? Preview { get; set; }

    [CliOption("--rate-limit-threshold-count")]
    public string? RateLimitThresholdCount { get; set; }

    [CliOption("--rate-limit-threshold-interval-sec")]
    public string? RateLimitThresholdIntervalSec { get; set; }

    [CliOption("--recaptcha-action-site-keys")]
    public string[]? RecaptchaActionSiteKeys { get; set; }

    [CliOption("--recaptcha-session-site-keys")]
    public string[]? RecaptchaSessionSiteKeys { get; set; }

    [CliOption("--redirect-target")]
    public string? RedirectTarget { get; set; }

    [CliOption("--redirect-type")]
    public string? RedirectType { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--request-headers-to-add")]
    public string[]? RequestHeadersToAdd { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }
}