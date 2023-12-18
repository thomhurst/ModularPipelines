using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "deployment", "create")]
public record AzIotDuDeviceDeploymentCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--un")] string Un,
[property: CommandSwitch("--up")] string Up,
[property: CommandSwitch("--update-version")] string UpdateVersion
) : AzOptions
{
    [CommandSwitch("--failed-count")]
    public int? FailedCount { get; set; }

    [CommandSwitch("--failed-percentage")]
    public string? FailedPercentage { get; set; }

    [CommandSwitch("--rbun")]
    public string? Rbun { get; set; }

    [CommandSwitch("--rbup")]
    public string? Rbup { get; set; }

    [CommandSwitch("--rbuv")]
    public string? Rbuv { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}