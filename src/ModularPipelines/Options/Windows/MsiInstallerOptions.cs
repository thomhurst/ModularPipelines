using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

[ExcludeFromCodeCoverage]
public record MsiInstallerOptions([property: CliOption("/package")] string MsiPath) : CommandLineToolOptions("msiexec.exe")
{
    [CliFlag("/exenoui")]
    public virtual bool? DisableUserInterface { get; set; } = true;

    [CliFlag("/norestart")]
    public virtual bool NoRestart { get; init; } = true;

    [CliFlag("/restartapplications")]
    public virtual bool RestartApplications { get; init; } = true;

    [CliFlag("/qn")]
    internal virtual bool? DisableUserInterface2 => DisableUserInterface;

    [CliFlag("/quiet")]
    internal virtual bool? DisableUserInterface3 => DisableUserInterface;

    [CliFlag("/silent")]
    internal virtual bool? DisableUserInterface4 => DisableUserInterface;

    [CliFlag("/verysilent")]
    internal virtual bool? DisableUserInterface5 => DisableUserInterface;

    [CliFlag("/suppressmsgboxes")]
    internal virtual bool? DisableUserInterface6 => DisableUserInterface;

    [CliFlag("/passive")]
    internal virtual bool? DisableUserInterface7 => DisableUserInterface;

    [CliFlag("/sp-")]
    internal virtual bool? DisableUserInterface8 => DisableUserInterface;
}