using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "deployment", "create")]
public record AzIotDuDeviceDeploymentCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--gid")] string Gid,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--un")] string Un,
[property: CliOption("--up")] string Up,
[property: CliOption("--update-version")] string UpdateVersion
) : AzOptions
{
    [CliOption("--failed-count")]
    public int? FailedCount { get; set; }

    [CliOption("--failed-percentage")]
    public string? FailedPercentage { get; set; }

    [CliOption("--rbun")]
    public string? Rbun { get; set; }

    [CliOption("--rbup")]
    public string? Rbup { get; set; }

    [CliOption("--rbuv")]
    public string? Rbuv { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}