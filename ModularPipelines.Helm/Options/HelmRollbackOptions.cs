using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

public record HelmRollbackOptions : HelmOptions
{
    [CommandLongSwitch("cleanup-on-fail", SwitchValueSeparator = " ")]
    public string? CleanupOnFail { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("help")]
    public bool? Help { get; set; }

    [CommandLongSwitch("history-max", SwitchValueSeparator = " ")]
    public int? HistoryMax { get; set; }

    [BooleanCommandSwitch("no-hooks")]
    public bool? NoHooks { get; set; }

    [CommandLongSwitch("recreate-pods", SwitchValueSeparator = " ")]
    public string? RecreatePods { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("wait")]
    public bool? Wait { get; set; }

    [CommandLongSwitch("wait-for-jobs", SwitchValueSeparator = " ")]
    public string? WaitForJobs { get; set; }
}
