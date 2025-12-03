using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "server-link", "create")]
public record AzRedisServerLinkCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--replication-role")] string ReplicationRole,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-to-link")] string ServerToLink
) : AzOptions;