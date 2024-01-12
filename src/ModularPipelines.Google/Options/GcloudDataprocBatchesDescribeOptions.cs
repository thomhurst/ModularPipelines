using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "batches", "describe")]
public record GcloudDataprocBatchesDescribeOptions(
[property: PositionalArgument] string Batch,
[property: PositionalArgument] string Region
) : GcloudOptions;