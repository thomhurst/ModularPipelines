using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adp", "account", "data-pool", "create")]
public record AzAdpAccountDataPoolCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--data-pool-name")] string DataPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

