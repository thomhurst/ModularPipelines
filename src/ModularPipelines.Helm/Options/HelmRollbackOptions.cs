using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("rollback")]
[ExcludeFromCodeCoverage]
public record HelmRollbackOptions : HelmOptions
{
    [BooleanCommandSwitch("--cleanup-on-fail")]
    public virtual bool? CleanupOnFail { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--history-max", SwitchValueSeparator = " ")]
    public int? HistoryMax { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [BooleanCommandSwitch("--recreate-pods")]
    public virtual bool? RecreatePods { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--wait-for-jobs")]
    public virtual bool? WaitForJobs { get; set; }
}