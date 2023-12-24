using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "delete-tls-inspection-configuration")]
public record AwsNetworkFirewallDeleteTlsInspectionConfigurationOptions : AwsOptions
{
    [CommandSwitch("--tls-inspection-configuration-arn")]
    public string? TlsInspectionConfigurationArn { get; set; }

    [CommandSwitch("--tls-inspection-configuration-name")]
    public string? TlsInspectionConfigurationName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}