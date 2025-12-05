using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "list")]
public record GcloudDataprocClustersListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}