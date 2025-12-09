using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "install-patches")]
public record AzVmInstallPatchesOptions(
[property: CliOption("--maximum-duration")] string MaximumDuration,
[property: CliOption("--reboot-setting")] string RebootSetting
) : AzOptions
{
    [CliOption("--classifications-to-include-linux")]
    public string? ClassificationsToIncludeLinux { get; set; }

    [CliOption("--classifications-to-include-win")]
    public string? ClassificationsToIncludeWin { get; set; }

    [CliFlag("--exclude-kbs-requiring-reboot")]
    public bool? ExcludeKbsRequiringReboot { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kb-numbers-to-exclude")]
    public string? KbNumbersToExclude { get; set; }

    [CliOption("--kb-numbers-to-include")]
    public string? KbNumbersToInclude { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--package-name-masks-to-exclude")]
    public string? PackageNameMasksToExclude { get; set; }

    [CliOption("--package-name-masks-to-include")]
    public string? PackageNameMasksToInclude { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}