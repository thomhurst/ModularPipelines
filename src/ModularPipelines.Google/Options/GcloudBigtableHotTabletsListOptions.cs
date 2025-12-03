using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "hot-tablets", "list")]
public record GcloudBigtableHotTabletsListOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}