using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "group", "list")]
public record AzIotDuDeviceGroupListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}