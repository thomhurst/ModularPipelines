using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agent-pools", "delete")]
public record GcloudTransferAgentPoolsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;