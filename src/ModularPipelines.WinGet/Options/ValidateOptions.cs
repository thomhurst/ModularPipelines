using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("validate")]
public record ValidateOptions(
    [property: CliOption("--manifest")] string Manifest
) : WingetOptions
{
    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}