using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

[ExcludeFromCodeCoverage]
public record ExeInstallerOptions(string ExePath) : CommandLineToolOptions(ExePath)
{
    [BooleanCommandSwitch("/exenoui")]
    public bool? DisableUserInterface { get; set; } = true;

    [BooleanCommandSwitch("/qn")] 
    internal bool? DisableUserInterface2 => DisableUserInterface;
    
    [BooleanCommandSwitch("/quiet")]
    internal bool? DisableUserInterface3 => DisableUserInterface;
    
    [BooleanCommandSwitch("/silent")]
    internal bool? DisableUserInterface4 => DisableUserInterface;

    [BooleanCommandSwitch("/verysilent")]
    internal bool? DisableUserInterface5 => DisableUserInterface;

    [BooleanCommandSwitch("/suppressmsgboxes")]
    internal bool DisableUserInterface6 { get; init; } = true;

    [BooleanCommandSwitch("/passive")]
    internal bool DisableUserInterface7 { get; init; } = true;

    [BooleanCommandSwitch("/norestart")]
    public bool NoRestart { get; init; } = true;
}
