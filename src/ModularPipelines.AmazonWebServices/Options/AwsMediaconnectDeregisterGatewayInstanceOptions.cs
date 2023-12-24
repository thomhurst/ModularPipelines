using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "deregister-gateway-instance")]
public record AwsMediaconnectDeregisterGatewayInstanceOptions(
[property: CommandSwitch("--gateway-instance-arn")] string GatewayInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}