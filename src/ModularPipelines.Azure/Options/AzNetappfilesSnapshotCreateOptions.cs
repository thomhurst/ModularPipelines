using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "snapshot", "create")]
public record AzNetappfilesSnapshotCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-name")] string VolumeName
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}