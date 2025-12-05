using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "describe-gateway-instance")]
public record AwsMediaconnectDescribeGatewayInstanceOptions(
[property: CliOption("--gateway-instance-arn")] string GatewayInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}