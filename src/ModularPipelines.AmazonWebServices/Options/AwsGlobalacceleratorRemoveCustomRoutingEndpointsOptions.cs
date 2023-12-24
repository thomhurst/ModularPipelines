using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "remove-custom-routing-endpoints")]
public record AwsGlobalacceleratorRemoveCustomRoutingEndpointsOptions(
[property: CommandSwitch("--endpoint-ids")] string[] EndpointIds,
[property: CommandSwitch("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}