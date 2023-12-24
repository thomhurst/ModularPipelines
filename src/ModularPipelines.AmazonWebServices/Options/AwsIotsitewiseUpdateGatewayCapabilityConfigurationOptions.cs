using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-gateway-capability-configuration")]
public record AwsIotsitewiseUpdateGatewayCapabilityConfigurationOptions(
[property: CommandSwitch("--gateway-id")] string GatewayId,
[property: CommandSwitch("--capability-namespace")] string CapabilityNamespace,
[property: CommandSwitch("--capability-configuration")] string CapabilityConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}