using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "keys", "create")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysCreateOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkloadIdentityPool,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--use")] string Use
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}