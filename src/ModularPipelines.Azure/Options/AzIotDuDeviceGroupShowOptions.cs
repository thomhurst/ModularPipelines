using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "group", "show")]
public record AzIotDuDeviceGroupShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliFlag("--best-updates")]
    public bool? BestUpdates { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--update-compliance")]
    public bool? UpdateCompliance { get; set; }
}