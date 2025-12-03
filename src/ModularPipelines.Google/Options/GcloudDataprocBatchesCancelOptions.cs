using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "batches", "cancel")]
public record GcloudDataprocBatchesCancelOptions(
[property: CliArgument] string Batch,
[property: CliArgument] string Region
) : GcloudOptions;