using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "delete")]
public record GcloudIamWorkloadIdentityPoolsDeleteOptions(
[property: PositionalArgument] string WorkloadIdentityPool,
[property: PositionalArgument] string Location
) : GcloudOptions;