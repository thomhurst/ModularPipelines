using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-gateway-instance")]
public record AwsMediaconnectUpdateGatewayInstanceOptions(
[property: CommandSwitch("--gateway-instance-arn")] string GatewayInstanceArn
) : AwsOptions
{
    [CommandSwitch("--bridge-placement")]
    public string? BridgePlacement { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}