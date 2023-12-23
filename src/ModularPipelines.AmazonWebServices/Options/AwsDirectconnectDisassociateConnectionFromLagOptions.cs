using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "disassociate-connection-from-lag")]
public record AwsDirectconnectDisassociateConnectionFromLagOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--lag-id")] string LagId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}