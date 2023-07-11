using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("uninstall")]
public record HelmUninstallOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [CommandEqualsSeparatorSwitch("--description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--keep-history")]
    public bool? KeepHistory { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public bool? NoHooks { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

}
