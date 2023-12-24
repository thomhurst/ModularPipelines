using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "create-tls-inspection-configuration")]
public record AwsNetworkFirewallCreateTlsInspectionConfigurationOptions(
[property: CommandSwitch("--tls-inspection-configuration-name")] string TlsInspectionConfigurationName,
[property: CommandSwitch("--tls-inspection-configuration")] string TlsInspectionConfiguration
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}