using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "replication", "get")]
public record GcloudSecretsReplicationGetOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions;