using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "subvolume", "list")]
public record AzNetappfilesSubvolumeListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-name")] string VolumeName
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--subvolume-name")]
    public string? SubvolumeName { get; set; }
}

