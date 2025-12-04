using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "deployment", "show")]
public record AzIotDuDeviceDeploymentShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--cid")]
    public string? Cid { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--status")]
    public bool? Status { get; set; }
}