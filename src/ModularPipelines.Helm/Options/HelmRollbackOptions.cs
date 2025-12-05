using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("rollback")]
[ExcludeFromCodeCoverage]
public record HelmRollbackOptions : HelmOptions
{
    [CliFlag("--cleanup-on-fail")]
    public virtual bool? CleanupOnFail { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--history-max")]
    public virtual int? HistoryMax { get; set; }

    [CliFlag("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CliFlag("--recreate-pods")]
    public virtual bool? RecreatePods { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--wait-for-jobs")]
    public virtual bool? WaitForJobs { get; set; }
}