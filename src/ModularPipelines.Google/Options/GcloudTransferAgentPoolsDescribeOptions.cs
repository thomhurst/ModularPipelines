using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agent-pools", "describe")]
public record GcloudTransferAgentPoolsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;