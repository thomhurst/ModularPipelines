using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "batches", "list")]
public record GcloudDataprocBatchesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}