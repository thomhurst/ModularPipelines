using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "get-iam-policy")]
public record GcloudIamWorkforcePoolsGetIamPolicyOptions(
[property: PositionalArgument] string WorkforcePool,
[property: PositionalArgument] string Location
) : GcloudOptions;