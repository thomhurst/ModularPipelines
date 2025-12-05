using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "delete-custom-routing-listener")]
public record AwsGlobalacceleratorDeleteCustomRoutingListenerOptions(
[property: CliOption("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}