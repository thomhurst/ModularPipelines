using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "keys", "list")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysListOptions(
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--workload-identity-pool")] string WorkloadIdentityPool
) : GcloudOptions
{
    [BooleanCommandSwitch("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}