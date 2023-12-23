using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "update-custom-routing-listener")]
public record AwsGlobalacceleratorUpdateCustomRoutingListenerOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn,
[property: CommandSwitch("--port-ranges")] string[] PortRanges
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}