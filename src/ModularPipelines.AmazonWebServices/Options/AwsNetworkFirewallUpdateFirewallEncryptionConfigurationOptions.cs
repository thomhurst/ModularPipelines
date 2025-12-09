using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-firewall-encryption-configuration")]
public record AwsNetworkFirewallUpdateFirewallEncryptionConfigurationOptions : AwsOptions
{
    [CliOption("--update-token")]
    public string? UpdateToken { get; set; }

    [CliOption("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CliOption("--firewall-name")]
    public string? FirewallName { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}