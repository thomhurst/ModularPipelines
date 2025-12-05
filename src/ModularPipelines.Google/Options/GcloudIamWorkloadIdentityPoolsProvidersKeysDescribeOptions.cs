using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "keys", "describe")]
public record GcloudIamWorkloadIdentityPoolsProvidersKeysDescribeOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkloadIdentityPool
) : GcloudOptions;