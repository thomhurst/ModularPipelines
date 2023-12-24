using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "associate-hosted-connection")]
public record AwsDirectconnectAssociateHostedConnectionOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--parent-connection-id")] string ParentConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}