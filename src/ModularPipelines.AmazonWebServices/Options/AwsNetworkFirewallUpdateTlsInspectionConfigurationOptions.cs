using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "update-tls-inspection-configuration")]
public record AwsNetworkFirewallUpdateTlsInspectionConfigurationOptions(
[property: CommandSwitch("--update-token")] string UpdateToken
) : AwsOptions
{
    [CommandSwitch("--tls-inspection-configuration-arn")]
    public string? TlsInspectionConfigurationArn { get; set; }

    [CommandSwitch("--tls-inspection-configuration-name")]
    public string? TlsInspectionConfigurationName { get; set; }

    [CommandSwitch("--tls-inspection-configuration")]
    public string? TlsInspectionConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}