using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "undelete")]
public record GcloudIamWorkloadIdentityPoolsUndeleteOptions(
[property: PositionalArgument] string WorkloadIdentityPool,
[property: PositionalArgument] string Location
) : GcloudOptions;