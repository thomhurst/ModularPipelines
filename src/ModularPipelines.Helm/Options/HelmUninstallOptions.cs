using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("uninstall")]
[ExcludeFromCodeCoverage]
public record HelmUninstallOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [CommandEqualsSeparatorSwitch("--description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--keep-history")]
    public virtual bool? KeepHistory { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }
}