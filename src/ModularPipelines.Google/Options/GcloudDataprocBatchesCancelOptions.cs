using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "batches", "cancel")]
public record GcloudDataprocBatchesCancelOptions(
[property: PositionalArgument] string Batch,
[property: PositionalArgument] string Region
) : GcloudOptions;