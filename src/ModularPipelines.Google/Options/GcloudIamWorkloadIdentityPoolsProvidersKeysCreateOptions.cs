using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "keys", "create")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysCreateOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string WorkloadIdentityPool,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--use")] string Use
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}