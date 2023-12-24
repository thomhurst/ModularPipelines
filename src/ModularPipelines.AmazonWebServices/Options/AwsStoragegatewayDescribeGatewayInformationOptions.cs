using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "describe-gateway-information")]
public record AwsStoragegatewayDescribeGatewayInformationOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}