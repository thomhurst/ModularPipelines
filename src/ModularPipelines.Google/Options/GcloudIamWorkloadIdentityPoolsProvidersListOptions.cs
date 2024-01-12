using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "list")]
public record GcloudIamWorkloadIdentityPoolsProvidersListOptions(
[property: CommandSwitch("--workload-identity-pool")] string WorkloadIdentityPool,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}