using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agent-pools", "delete")]
public record GcloudTransferAgentPoolsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;