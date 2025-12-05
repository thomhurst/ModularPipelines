using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "log", "show")]
public record AzIotDuDeviceLogShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--lcid")] string Lcid
) : AzOptions
{
    [CliFlag("--detailed")]
    public bool? Detailed { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}