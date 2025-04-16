using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("workload", "repair")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadRepairOptions : DotNetOptions
{
    [BooleanCommandSwitch("--configfile")]
    public virtual bool? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CommandSwitch("--source")]
    public virtual IEnumerable<string>? Source { get; set; }

    [CommandSwitch("--temp-dir")]
    public virtual string? TempDir { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
