using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "operations", "describe")]
public record GcloudIamWorkloadIdentityPoolsOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkloadIdentityPool
) : GcloudOptions;