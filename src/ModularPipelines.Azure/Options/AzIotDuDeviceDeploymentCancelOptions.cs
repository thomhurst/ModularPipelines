using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "deployment", "cancel")]
public record AzIotDuDeviceDeploymentCancelOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cid")] string Cid,
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}