using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "subvolume", "create")]
public record AzNetappfilesSubvolumeCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subvolume-name")] string SubvolumeName,
[property: CommandSwitch("--volume-name")] string VolumeName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parent-path")]
    public string? ParentPath { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}

