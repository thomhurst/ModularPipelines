using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "describe-gateway-capability-configuration")]
public record AwsIotsitewiseDescribeGatewayCapabilityConfigurationOptions(
[property: CliOption("--gateway-id")] string GatewayId,
[property: CliOption("--capability-namespace")] string CapabilityNamespace
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}