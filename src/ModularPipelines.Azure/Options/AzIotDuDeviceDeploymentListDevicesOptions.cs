using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "deployment", "list-devices")]
public record AzIotDuDeviceDeploymentListDevicesOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cid")] string Cid,
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}