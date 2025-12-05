using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "describe")]
public record GcloudDataprocOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;