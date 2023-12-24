using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-delivery-destination-policy")]
public record AwsLogsPutDeliveryDestinationPolicyOptions(
[property: CommandSwitch("--delivery-destination-name")] string DeliveryDestinationName,
[property: CommandSwitch("--delivery-destination-policy")] string DeliveryDestinationPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}