using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "class", "update")]
public record AzIotDuDeviceClassUpdateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cid")] string Cid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}