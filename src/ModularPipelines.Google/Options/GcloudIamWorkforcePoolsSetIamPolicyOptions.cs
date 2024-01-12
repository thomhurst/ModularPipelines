using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "set-iam-policy")]
public record GcloudIamWorkforcePoolsSetIamPolicyOptions(
[property: PositionalArgument] string WorkforcePool,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;