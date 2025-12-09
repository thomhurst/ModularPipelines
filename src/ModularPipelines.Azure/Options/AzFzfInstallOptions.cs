using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fzf", "install")]
public record AzFzfInstallOptions : AzOptions
{
    [CliOption("--install-dir")]
    public string? InstallDir { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}