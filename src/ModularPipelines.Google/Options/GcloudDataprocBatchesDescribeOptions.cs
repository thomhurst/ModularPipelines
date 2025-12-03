using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "batches", "describe")]
public record GcloudDataprocBatchesDescribeOptions(
[property: CliArgument] string Batch,
[property: CliArgument] string Region
) : GcloudOptions;