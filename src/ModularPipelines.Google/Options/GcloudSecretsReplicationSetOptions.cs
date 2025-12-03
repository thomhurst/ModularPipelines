using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "replication", "set")]
public record GcloudSecretsReplicationSetOptions(
[property: CliArgument] string Secret,
[property: CliOption("--replication-policy-file")] string ReplicationPolicyFile
) : GcloudOptions;