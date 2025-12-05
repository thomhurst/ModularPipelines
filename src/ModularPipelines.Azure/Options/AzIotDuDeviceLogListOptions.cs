using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "log", "list")]
public record AzIotDuDeviceLogListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}