using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "operations", "describe")]
public record GcloudBigtableOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;