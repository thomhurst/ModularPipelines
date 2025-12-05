using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-configuration-set-delivery-options")]
public record AwsSesv2PutConfigurationSetDeliveryOptionsOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CliOption("--tls-policy")]
    public string? TlsPolicy { get; set; }

    [CliOption("--sending-pool-name")]
    public string? SendingPoolName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}