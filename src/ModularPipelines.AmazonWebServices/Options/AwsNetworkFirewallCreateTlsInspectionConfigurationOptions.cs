using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "create-tls-inspection-configuration")]
public record AwsNetworkFirewallCreateTlsInspectionConfigurationOptions(
[property: CliOption("--tls-inspection-configuration-name")] string TlsInspectionConfigurationName,
[property: CliOption("--tls-inspection-configuration")] string TlsInspectionConfiguration
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