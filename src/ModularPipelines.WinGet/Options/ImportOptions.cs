using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("import")]
public record ImportOptions(
    [property: CliOption("--import-file")] string ImportFile
) : WingetOptions
{
    [CliFlag("--ignore-unavailable")]
    public virtual bool? IgnoreUnavailable { get; set; }

    [CliFlag("--ignore-versions")]
    public virtual bool? IgnoreVersions { get; set; }

    [CliFlag("--no-upgrade")]
    public virtual bool? NoUpgrade { get; set; }

    [CliFlag("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CliOption("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}