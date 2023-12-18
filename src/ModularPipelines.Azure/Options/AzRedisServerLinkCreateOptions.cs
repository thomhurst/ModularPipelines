using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "server-link", "create")]
public record AzRedisServerLinkCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--replication-role")] string ReplicationRole,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-to-link")] string ServerToLink
) : AzOptions;