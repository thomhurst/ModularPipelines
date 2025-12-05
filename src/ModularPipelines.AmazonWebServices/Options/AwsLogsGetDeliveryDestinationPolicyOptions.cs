using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "get-delivery-destination-policy")]
public record AwsLogsGetDeliveryDestinationPolicyOptions(
[property: CliOption("--delivery-destination-name")] string DeliveryDestinationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}