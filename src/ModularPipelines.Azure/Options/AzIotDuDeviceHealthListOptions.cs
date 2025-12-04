using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "health", "list")]
public record AzIotDuDeviceHealthListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--filter")] string Filter,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}