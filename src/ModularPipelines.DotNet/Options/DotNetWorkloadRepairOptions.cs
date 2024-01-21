using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("workload", "repair")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadRepairOptions : DotNetOptions
{
    [BooleanCommandSwitch("--configfile")]
    public bool? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--source")]
    public IEnumerable<string>? Source { get; set; }

    [CommandSwitch("--temp-dir")]
    public string? TempDir { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
