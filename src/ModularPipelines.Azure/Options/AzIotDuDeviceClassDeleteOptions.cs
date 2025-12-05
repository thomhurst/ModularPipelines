using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "class", "delete")]
public record AzIotDuDeviceClassDeleteOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cid")] string Cid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--gid")]
    public string? Gid { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}