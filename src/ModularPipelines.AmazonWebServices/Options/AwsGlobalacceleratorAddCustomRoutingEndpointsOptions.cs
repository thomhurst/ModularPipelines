using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "add-custom-routing-endpoints")]
public record AwsGlobalacceleratorAddCustomRoutingEndpointsOptions(
[property: CommandSwitch("--endpoint-configurations")] string[] EndpointConfigurations,
[property: CommandSwitch("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}