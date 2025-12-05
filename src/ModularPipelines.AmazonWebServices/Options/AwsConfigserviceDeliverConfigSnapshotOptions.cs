using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "deliver-config-snapshot")]
public record AwsConfigserviceDeliverConfigSnapshotOptions(
[property: CliOption("--delivery-channel-name")] string DeliveryChannelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}