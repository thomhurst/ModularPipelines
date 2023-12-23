using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "create-custom-routing-endpoint-group")]
public record AwsGlobalacceleratorCreateCustomRoutingEndpointGroupOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn,
[property: CommandSwitch("--endpoint-group-region")] string EndpointGroupRegion,
[property: CommandSwitch("--destination-configurations")] string[] DestinationConfigurations
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}