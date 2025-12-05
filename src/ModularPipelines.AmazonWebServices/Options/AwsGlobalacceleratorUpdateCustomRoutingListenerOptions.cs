using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "update-custom-routing-listener")]
public record AwsGlobalacceleratorUpdateCustomRoutingListenerOptions(
[property: CliOption("--listener-arn")] string ListenerArn,
[property: CliOption("--port-ranges")] string[] PortRanges
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}