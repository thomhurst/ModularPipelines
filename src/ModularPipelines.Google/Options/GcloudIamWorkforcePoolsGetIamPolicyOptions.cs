using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "get-iam-policy")]
public record GcloudIamWorkforcePoolsGetIamPolicyOptions(
[property: CliArgument] string WorkforcePool,
[property: CliArgument] string Location
) : GcloudOptions;