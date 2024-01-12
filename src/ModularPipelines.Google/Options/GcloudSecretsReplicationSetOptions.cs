using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "replication", "set")]
public record GcloudSecretsReplicationSetOptions(
[property: PositionalArgument] string Secret,
[property: CommandSwitch("--replication-policy-file")] string ReplicationPolicyFile
) : GcloudOptions;