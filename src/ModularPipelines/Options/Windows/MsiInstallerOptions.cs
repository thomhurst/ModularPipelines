using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

[ExcludeFromCodeCoverage]
public record MsiInstallerOptions([property: CommandSwitch("/package")] string MsiPath) : CommandLineToolOptions("msiexec.exe")
{
    [BooleanCommandSwitch("/quiet")]
    public bool Quiet { get; } = true;

    [BooleanCommandSwitch("/qn")]
    public bool? DisableUserInterface { get; set; } = true;

    [BooleanCommandSwitch("/norestart")]
    public bool NoRestart { get; } = true;
}
