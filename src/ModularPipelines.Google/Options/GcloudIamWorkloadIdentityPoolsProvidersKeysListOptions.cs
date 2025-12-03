using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "keys", "list")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysListOptions(
[property: CliOption("--provider")] string Provider,
[property: CliOption("--location")] string Location,
[property: CliOption("--workload-identity-pool")] string WorkloadIdentityPool
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}