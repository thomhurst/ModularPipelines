using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "providers", "operations", "describe")]
public record GcloudIamWorkloadIdentityPoolsProvidersOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string WorkloadIdentityPool
) : GcloudOptions;