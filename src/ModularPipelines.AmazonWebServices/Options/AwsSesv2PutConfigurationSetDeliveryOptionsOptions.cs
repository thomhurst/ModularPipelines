using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-configuration-set-delivery-options")]
public record AwsSesv2PutConfigurationSetDeliveryOptionsOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--tls-policy")]
    public string? TlsPolicy { get; set; }

    [CommandSwitch("--sending-pool-name")]
    public string? SendingPoolName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}