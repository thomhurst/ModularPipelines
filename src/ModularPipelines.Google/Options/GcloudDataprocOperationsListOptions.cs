using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "list")]
public record GcloudDataprocOperationsListOptions : GcloudOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--state-filter")]
    public string? StateFilter { get; set; }
}