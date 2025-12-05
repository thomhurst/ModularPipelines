using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-configuration-set-delivery-options")]
public record AwsPinpointEmailPutConfigurationSetDeliveryOptionsOptions(
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