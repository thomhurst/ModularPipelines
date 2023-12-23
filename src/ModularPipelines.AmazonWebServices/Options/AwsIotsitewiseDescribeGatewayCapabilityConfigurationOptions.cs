using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "describe-gateway-capability-configuration")]
public record AwsIotsitewiseDescribeGatewayCapabilityConfigurationOptions(
[property: CommandSwitch("--gateway-id")] string GatewayId,
[property: CommandSwitch("--capability-namespace")] string CapabilityNamespace
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}