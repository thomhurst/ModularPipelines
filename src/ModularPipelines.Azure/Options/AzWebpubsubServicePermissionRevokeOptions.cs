using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webpubsub", "service", "permission", "revoke")]
public record AzWebpubsubServicePermissionRevokeOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--permission")] string Permission
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}