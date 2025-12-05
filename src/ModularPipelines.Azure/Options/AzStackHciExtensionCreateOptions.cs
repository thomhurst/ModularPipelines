using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci", "extension", "create")]
public record AzStackHciExtensionCreateOptions(
[property: CliOption("--arc-setting-name")] string ArcSettingName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--extension-name")] string ExtensionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}