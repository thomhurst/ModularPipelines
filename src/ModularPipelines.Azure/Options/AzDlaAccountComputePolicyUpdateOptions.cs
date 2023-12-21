using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "compute-policy", "update")]
public record AzDlaAccountComputePolicyUpdateOptions(
[property: CommandSwitch("--compute-policy-name")] string ComputePolicyName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-dop-per-job")]
    public string? MaxDopPerJob { get; set; }

    [CommandSwitch("--min-priority-per-job")]
    public string? MinPriorityPerJob { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}