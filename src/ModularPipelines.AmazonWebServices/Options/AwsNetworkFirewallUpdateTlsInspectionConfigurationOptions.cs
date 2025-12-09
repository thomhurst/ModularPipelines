using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-tls-inspection-configuration")]
public record AwsNetworkFirewallUpdateTlsInspectionConfigurationOptions(
[property: CliOption("--update-token")] string UpdateToken
) : AwsOptions
{
    [CliOption("--tls-inspection-configuration-arn")]
    public string? TlsInspectionConfigurationArn { get; set; }

    [CliOption("--tls-inspection-configuration-name")]
    public string? TlsInspectionConfigurationName { get; set; }

    [CliOption("--tls-inspection-configuration")]
    public string? TlsInspectionConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}