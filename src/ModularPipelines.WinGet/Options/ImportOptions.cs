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
    public bool? IgnoreUnavailable { get; set; }

    [BooleanCommandSwitch("--ignore-versions")]
    public bool? IgnoreVersions { get; set; }

    [BooleanCommandSwitch("--no-upgrade")]
    public bool? NoUpgrade { get; set; }

    [BooleanCommandSwitch("--accept-package-agreements")]
    public bool? AcceptPackageAgreements { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public bool? DisableInteractivity { get; set; }
}