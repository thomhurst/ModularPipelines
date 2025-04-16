using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

[ExcludeFromCodeCoverage]
public record ExeInstallerOptions(string ExePath) : CommandLineToolOptions(ExePath)
{
    [BooleanCommandSwitch("/exenoui")]
    public virtual bool? DisableUserInterface { get; set; } = true;

    [BooleanCommandSwitch("/norestart")]
    public virtual bool NoRestart { get; init; } = true;

    [BooleanCommandSwitch("/restartapplications")]
    public virtual bool RestartApplications { get; init; } = true;

    [BooleanCommandSwitch("/qn")]
    internal virtual bool? DisableUserInterface2 => DisableUserInterface;

    [BooleanCommandSwitch("/quiet")]
    internal virtual bool? DisableUserInterface3 => DisableUserInterface;

    [BooleanCommandSwitch("/silent")]
    internal virtual bool? DisableUserInterface4 => DisableUserInterface;

    [BooleanCommandSwitch("/verysilent")]
    internal virtual bool? DisableUserInterface5 => DisableUserInterface;

    [BooleanCommandSwitch("/suppressmsgboxes")]
    internal virtual bool? DisableUserInterface6 => DisableUserInterface;

    [BooleanCommandSwitch("/passive")]
    internal virtual bool? DisableUserInterface7 => DisableUserInterface;

    [BooleanCommandSwitch("/sp-")]
    internal virtual bool? DisableUserInterface8 => DisableUserInterface;
}