using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "compute-policy", "create")]
public record AzDlaAccountComputePolicyCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--compute-policy-name")] string ComputePolicyName,
[property: CommandSwitch("--object-id")] string ObjectId,
[property: CommandSwitch("--object-type")] string ObjectType
) : AzOptions
{
    [CommandSwitch("--max-dop-per-job")]
    public string? MaxDopPerJob { get; set; }

    [CommandSwitch("--min-priority-per-job")]
    public string? MinPriorityPerJob { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}