using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import")]
public record ImportOptions(
    [property: CommandSwitch("--import-file")] string ImportFile
) : WingetOptions
{
    [BooleanCommandSwitch("--ignore-unavailable")]
    public virtual bool? IgnoreUnavailable { get; set; }

    [BooleanCommandSwitch("--ignore-versions")]
    public virtual bool? IgnoreVersions { get; set; }

    [BooleanCommandSwitch("--no-upgrade")]
    public virtual bool? NoUpgrade { get; set; }

    [BooleanCommandSwitch("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}