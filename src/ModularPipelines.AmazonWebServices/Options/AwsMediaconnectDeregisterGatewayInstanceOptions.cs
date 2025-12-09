using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "deregister-gateway-instance")]
public record AwsMediaconnectDeregisterGatewayInstanceOptions(
[property: CliOption("--gateway-instance-arn")] string GatewayInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}