using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fzf", "install")]
public record AzFzfInstallOptions : AzOptions
{
    [CommandSwitch("--install-dir")]
    public string? InstallDir { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}