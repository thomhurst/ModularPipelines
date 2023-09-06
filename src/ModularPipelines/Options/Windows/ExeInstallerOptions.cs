using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

[ExcludeFromCodeCoverage]
public record ExeInstallerOptions(string ExePath) : CommandLineToolOptions(ExePath)
{
    [BooleanCommandSwitch("/exenoui")]
    public bool? DisableUserInterface { get; set; } = true;

    [BooleanCommandSwitch("/qn")]
    public bool Quiet { get; } = true;

    [BooleanCommandSwitch("/norestart")]
    public bool NoRestart { get; } = true;
}
