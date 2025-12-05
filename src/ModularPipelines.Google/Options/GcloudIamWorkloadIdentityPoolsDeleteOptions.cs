using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "delete")]
public record GcloudIamWorkloadIdentityPoolsDeleteOptions(
[property: CliArgument] string WorkloadIdentityPool,
[property: CliArgument] string Location
) : GcloudOptions;