using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "cancel")]
public record GcloudDataprocOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;