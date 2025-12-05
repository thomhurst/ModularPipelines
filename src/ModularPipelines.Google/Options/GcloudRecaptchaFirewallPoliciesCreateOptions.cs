using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recaptcha", "firewall-policies", "create")]
public record GcloudRecaptchaFirewallPoliciesCreateOptions : GcloudOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }
}