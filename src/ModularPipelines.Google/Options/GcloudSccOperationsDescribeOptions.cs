using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "operations", "describe")]
public record GcloudSccOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Organization
) : GcloudOptions;