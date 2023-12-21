using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "force-reboot")]
public record AzRedisForceRebootOptions(
[property: CommandSwitch("--reboot-type")] string RebootType
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--shard-id")]
    public string? ShardId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}