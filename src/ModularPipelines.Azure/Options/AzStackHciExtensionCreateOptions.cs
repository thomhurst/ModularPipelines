using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci", "extension", "create")]
public record AzStackHciExtensionCreateOptions(
[property: CommandSwitch("--arc-setting-name")] string ArcSettingName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--extension-name")] string ExtensionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [BooleanCommandSwitch("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}