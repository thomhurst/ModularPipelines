using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "allocate-hosted-connection")]
public record AwsDirectconnectAllocateHostedConnectionOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--owner-account")] string OwnerAccount,
[property: CommandSwitch("--bandwidth")] string Bandwidth,
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--vlan")] int Vlan
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}