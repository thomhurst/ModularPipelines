using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("workload", "repair")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadRepairOptions : DotNetOptions
{
    [CliFlag("--configfile")]
    public virtual bool? Configfile { get; set; }

    [CliFlag("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CliFlag("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--source")]
    public virtual IEnumerable<string>? Source { get; set; }

    [CliOption("--temp-dir")]
    public virtual string? TempDir { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
