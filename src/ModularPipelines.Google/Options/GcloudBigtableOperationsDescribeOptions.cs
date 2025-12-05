using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "operations", "describe")]
public record GcloudBigtableOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;