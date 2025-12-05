using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-delivery-destination-policy")]
public record AwsLogsPutDeliveryDestinationPolicyOptions(
[property: CliOption("--delivery-destination-name")] string DeliveryDestinationName,
[property: CliOption("--delivery-destination-policy")] string DeliveryDestinationPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}