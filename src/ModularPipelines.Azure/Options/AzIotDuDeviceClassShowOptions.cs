using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "class", "show")]
public record AzIotDuDeviceClassShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cid")] string Cid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliFlag("--best-update")]
    public bool? BestUpdate { get; set; }

    [CliOption("--gid")]
    public string? Gid { get; set; }

    [CliFlag("--installable-updates")]
    public bool? InstallableUpdates { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--update-compliance")]
    public bool? UpdateCompliance { get; set; }
}