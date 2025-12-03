using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-firewall-policy")]
public record AwsNetworkFirewallUpdateFirewallPolicyOptions(
[property: CliOption("--update-token")] string UpdateToken
) : AwsOptions
{
    [CliOption("--firewall-policy-arn")]
    public string? FirewallPolicyArn { get; set; }

    [CliOption("--firewall-policy-name")]
    public string? FirewallPolicyName { get; set; }

    [CliOption("--firewall-policy")]
    public string? FirewallPolicy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}