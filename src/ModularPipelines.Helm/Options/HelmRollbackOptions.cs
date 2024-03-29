using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("rollback")]
[ExcludeFromCodeCoverage]
public record HelmRollbackOptions : HelmOptions
{
    [BooleanCommandSwitch("--cleanup-on-fail")]
    public bool? CleanupOnFail { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--history-max", SwitchValueSeparator = " ")]
    public int? HistoryMax { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public bool? NoHooks { get; set; }

    [BooleanCommandSwitch("--recreate-pods")]
    public bool? RecreatePods { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--wait-for-jobs")]
    public bool? WaitForJobs { get; set; }
}