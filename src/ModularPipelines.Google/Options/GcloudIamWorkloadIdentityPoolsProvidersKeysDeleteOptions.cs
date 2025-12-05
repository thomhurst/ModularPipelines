using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "keys", "delete")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysDeleteOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkloadIdentityPool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}