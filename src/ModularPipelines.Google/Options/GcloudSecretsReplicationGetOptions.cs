using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "replication", "get")]
public record GcloudSecretsReplicationGetOptions(
[property: CliArgument] string Secret
) : GcloudOptions;