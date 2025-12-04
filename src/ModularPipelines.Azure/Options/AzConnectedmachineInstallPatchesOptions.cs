using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine", "install-patches")]
public record AzConnectedmachineInstallPatchesOptions(
[property: CliOption("--maximum-duration")] string MaximumDuration,
[property: CliOption("--reboot-setting")] string RebootSetting
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--linux-parameters")]
    public string? LinuxParameters { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--windows-parameters")]
    public string? WindowsParameters { get; set; }
}