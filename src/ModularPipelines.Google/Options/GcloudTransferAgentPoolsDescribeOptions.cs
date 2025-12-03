using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agent-pools", "describe")]
public record GcloudTransferAgentPoolsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;