﻿using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("search", "repo")]
public record HelmSearchRepoOptions : HelmOptions
{
    [BooleanCommandSwitch("devel")]
    public bool? Devel { get; set; }

    [CommandLongSwitch("max-col-width", SwitchValueSeparator = " ")]
    public string? MaxColWidth { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("regexp")]
    public bool? Regexp { get; set; }

    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("versions")]
    public bool? Versions { get; set; }

}
