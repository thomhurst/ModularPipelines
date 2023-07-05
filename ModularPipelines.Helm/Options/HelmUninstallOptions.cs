using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

public record HelmUninstallOptions : HelmOptions
{
    [CommandLongSwitch( "cascade", SwitchValueSeparator = " " )]
    public string? Cascade { get; set; }

    [CommandLongSwitch( "description", SwitchValueSeparator = " " )]
    public string? Description { get; set; }

    [BooleanCommandSwitch( "dry-run" )]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch( "keep-history" )]
    public bool? KeepHistory { get; set; }

    [BooleanCommandSwitch( "no-hooks" )]
    public bool? NoHooks { get; set; }

    [CommandLongSwitch( "timeout", SwitchValueSeparator = " " )]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch( "wait" )]
    public bool? Wait { get; set; }
}
