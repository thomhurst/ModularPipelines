using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "cluster", "setting", "remove")]
public record AzSfClusterSettingRemoveOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--parameter")]
    public string? Parameter { get; set; }

    [CliOption("--section")]
    public string? Section { get; set; }

    [CliOption("--settings-section")]
    public string? SettingsSection { get; set; }
}