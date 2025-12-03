using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-gateway-capability-configuration")]
public record AwsIotsitewiseUpdateGatewayCapabilityConfigurationOptions(
[property: CliOption("--gateway-id")] string GatewayId,
[property: CliOption("--capability-namespace")] string CapabilityNamespace,
[property: CliOption("--capability-configuration")] string CapabilityConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}