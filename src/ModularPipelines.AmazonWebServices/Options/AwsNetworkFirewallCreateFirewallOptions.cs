using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "create-firewall")]
public record AwsNetworkFirewallCreateFirewallOptions(
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--firewall-policy-arn")] string FirewallPolicyArn,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--subnet-mappings")] string[] SubnetMappings
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