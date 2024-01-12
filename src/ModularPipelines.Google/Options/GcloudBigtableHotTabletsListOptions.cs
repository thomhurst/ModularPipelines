using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "hot-tablets", "list")]
public record GcloudBigtableHotTabletsListOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}