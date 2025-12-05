using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "operations", "describe")]
public record GcloudSccOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Organization
) : GcloudOptions;