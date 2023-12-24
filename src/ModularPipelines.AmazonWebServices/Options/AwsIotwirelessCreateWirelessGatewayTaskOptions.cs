using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "create-wireless-gateway-task")]
public record AwsIotwirelessCreateWirelessGatewayTaskOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--wireless-gateway-task-definition-id")] string WirelessGatewayTaskDefinitionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}