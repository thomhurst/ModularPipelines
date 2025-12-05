using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci", "arc-setting", "show")]
public record AzStackHciArcSettingShowOptions : AzOptions
{
    [CliOption("--arc-setting-name")]
    public string? ArcSettingName { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}