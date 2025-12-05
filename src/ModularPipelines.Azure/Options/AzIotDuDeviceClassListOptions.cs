using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "class", "list")]
public record AzIotDuDeviceClassListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--gid")]
    public string? Gid { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}