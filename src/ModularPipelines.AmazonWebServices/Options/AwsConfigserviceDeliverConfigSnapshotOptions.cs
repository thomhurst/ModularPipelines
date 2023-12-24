using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "deliver-config-snapshot")]
public record AwsConfigserviceDeliverConfigSnapshotOptions(
[property: CommandSwitch("--delivery-channel-name")] string DeliveryChannelName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}