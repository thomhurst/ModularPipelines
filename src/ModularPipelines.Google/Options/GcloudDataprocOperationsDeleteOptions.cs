using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "delete")]
public record GcloudDataprocOperationsDeleteOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;