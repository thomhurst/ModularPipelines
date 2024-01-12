using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "keys", "undelete")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysUndeleteOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string WorkloadIdentityPool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}