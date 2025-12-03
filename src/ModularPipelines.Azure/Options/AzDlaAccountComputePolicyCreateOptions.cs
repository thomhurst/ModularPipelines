using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "account", "compute-policy", "create")]
public record AzDlaAccountComputePolicyCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--compute-policy-name")] string ComputePolicyName,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--object-type")] string ObjectType
) : AzOptions
{
    [CliOption("--max-dop-per-job")]
    public string? MaxDopPerJob { get; set; }

    [CliOption("--min-priority-per-job")]
    public string? MinPriorityPerJob { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}