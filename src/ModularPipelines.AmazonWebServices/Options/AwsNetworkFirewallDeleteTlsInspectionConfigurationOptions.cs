using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "delete-tls-inspection-configuration")]
public record AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions : AwsOptions
{
    [CliOption("--tls-inspection-configuration-arn")]
    public string? TlsInspectionConfigurationArn { get; set; }

    [CliOption("--tls-inspection-configuration-name")]
    public string? TlsInspectionConfigurationName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}