using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy-intelligence", "troubleshoot-policy", "iam")]
public record GcloudPolicyIntelligenceTroubleshootPolicyIamOptions(
[property: CliArgument] string Resource,
[property: CliOption("--permission")] string Permission,
[property: CliOption("--principal-email")] string PrincipalEmail
) : GcloudOptions
{
    [CliOption("--destination-ip")]
    public string? DestinationIp { get; set; }

    [CliOption("--destination-port")]
    public string? DestinationPort { get; set; }

    [CliOption("--request-time")]
    public string? RequestTime { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-service")]
    public string? ResourceService { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}