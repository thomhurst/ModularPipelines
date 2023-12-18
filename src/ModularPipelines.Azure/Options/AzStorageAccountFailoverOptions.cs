using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "failover")]
public record AzStorageAccountFailoverOptions(
[property: CommandSwitch("--expiry")] string Expiry,
[property: CommandSwitch("--permissions")] string Permissions,
[property: CommandSwitch("--resource-types")] string ResourceTypes,
[property: CommandSwitch("--services")] string Services
) : AzOptions
{
    [CommandSwitch("--failover-type")]
    public string? FailoverType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}