using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "clusters", "list")]
public record GcloudBigtableClustersListOptions : GcloudOptions
{
    [CliOption("--instances")]
    public string[]? Instances { get; set; }
}