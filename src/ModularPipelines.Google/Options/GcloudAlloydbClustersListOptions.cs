using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "list")]
public record GcloudAlloydbClustersListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}