using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-wireless-gateway-task")]
public record AwsIotwirelessCreateWirelessGatewayTaskOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--wireless-gateway-task-definition-id")] string WirelessGatewayTaskDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}