using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "account", "compute-policy", "update")]
public record AzDlaAccountComputePolicyUpdateOptions(
[property: CliOption("--compute-policy-name")] string ComputePolicyName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-dop-per-job")]
    public string? MaxDopPerJob { get; set; }

    [CliOption("--min-priority-per-job")]
    public string? MinPriorityPerJob { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}