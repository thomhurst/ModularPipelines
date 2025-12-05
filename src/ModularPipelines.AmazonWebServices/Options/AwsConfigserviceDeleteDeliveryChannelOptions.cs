using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-delivery-channel")]
public record AwsConfigserviceDeleteDeliveryChannelOptions(
[property: CliOption("--delivery-channel-name")] string DeliveryChannelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}