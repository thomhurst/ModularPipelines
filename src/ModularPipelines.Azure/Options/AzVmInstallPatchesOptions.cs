using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "install-patches")]
public record AzVmInstallPatchesOptions(
[property: CommandSwitch("--maximum-duration")] string MaximumDuration,
[property: CommandSwitch("--reboot-setting")] string RebootSetting
) : AzOptions
{
    [CommandSwitch("--classifications-to-include-linux")]
    public string? ClassificationsToIncludeLinux { get; set; }

    [CommandSwitch("--classifications-to-include-win")]
    public string? ClassificationsToIncludeWin { get; set; }

    [BooleanCommandSwitch("--exclude-kbs-requiring-reboot")]
    public bool? ExcludeKbsRequiringReboot { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kb-numbers-to-exclude")]
    public string? KbNumbersToExclude { get; set; }

    [CommandSwitch("--kb-numbers-to-include")]
    public string? KbNumbersToInclude { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--package-name-masks-to-exclude")]
    public string? PackageNameMasksToExclude { get; set; }

    [CommandSwitch("--package-name-masks-to-include")]
    public string? PackageNameMasksToInclude { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}