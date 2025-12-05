using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "operations", "describe")]
public record GcloudIamWorkloadIdentityPoolsOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string WorkloadIdentityPool
) : GcloudOptions;