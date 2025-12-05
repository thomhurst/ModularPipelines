using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-delivery-destination-policy")]
public record AwsLogsDeleteDeliveryDestinationPolicyOptions(
[property: CliOption("--delivery-destination-name")] string DeliveryDestinationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}