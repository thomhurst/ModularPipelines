using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "batches", "wait")]
public record GcloudDataprocBatchesWaitOptions(
[property: CliArgument] string Batch,
[property: CliArgument] string Region
) : GcloudOptions;