using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "break-file-locks")]
public record AzNetappfilesVolumeBreakFileLocksOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--usage-threshold")] string UsageThreshold,
[property: CommandSwitch("--vnet")] string Vnet
) : AzOptions
{
    [CommandSwitch("--client-ip")]
    public string? ClientIp { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

