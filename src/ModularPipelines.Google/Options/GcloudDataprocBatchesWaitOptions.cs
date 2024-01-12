using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "batches", "wait")]
public record GcloudDataprocBatchesWaitOptions(
[property: PositionalArgument] string Batch,
[property: PositionalArgument] string Region
) : GcloudOptions;