using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "describe-custom-routing-listener")]
public record AwsGlobalacceleratorDescribeCustomRoutingListenerOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}