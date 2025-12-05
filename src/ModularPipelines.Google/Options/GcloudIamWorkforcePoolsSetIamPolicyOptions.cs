using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "set-iam-policy")]
public record GcloudIamWorkforcePoolsSetIamPolicyOptions(
[property: CliArgument] string WorkforcePool,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;