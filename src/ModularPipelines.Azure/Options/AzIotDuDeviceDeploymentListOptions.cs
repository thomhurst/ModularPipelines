using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "deployment", "list")]
public record AzIotDuDeviceDeploymentListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--cid")]
    public string? Cid { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}