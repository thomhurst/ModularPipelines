using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "describe")]
public record GcloudIamWorkloadIdentityPoolsDescribeOptions(
[property: CliArgument] string WorkloadIdentityPool,
[property: CliArgument] string Location
) : GcloudOptions;