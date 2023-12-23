using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "get-delivery-destination-policy")]
public record AwsLogsGetDeliveryDestinationPolicyOptions(
[property: CommandSwitch("--delivery-destination-name")] string DeliveryDestinationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}