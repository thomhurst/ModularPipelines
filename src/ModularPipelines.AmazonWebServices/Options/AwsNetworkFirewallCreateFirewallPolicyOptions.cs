using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "create-firewall-policy")]
public record AwsNetworkFirewallCreateFirewallPolicyOptions(
[property: CliOption("--firewall-policy-name")] string FirewallPolicyName,
[property: CliOption("--firewall-policy")] string FirewallPolicy
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}