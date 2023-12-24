using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-gateway")]
public record AwsIotsitewiseUpdateGatewayOptions(
[property: CommandSwitch("--gateway-id")] string GatewayId,
[property: CommandSwitch("--gateway-name")] string GatewayName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}