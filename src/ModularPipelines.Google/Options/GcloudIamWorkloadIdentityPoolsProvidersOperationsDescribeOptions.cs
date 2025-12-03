using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "operations", "describe")]
public record GcloudIamWorkloadIdentityPoolsProvidersOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkloadIdentityPool
) : GcloudOptions;