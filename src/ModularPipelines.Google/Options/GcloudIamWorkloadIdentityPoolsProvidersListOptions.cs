using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "list")]
public record GcloudIamWorkloadIdentityPoolsProvidersListOptions(
[property: CliOption("--workload-identity-pool")] string WorkloadIdentityPool,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}