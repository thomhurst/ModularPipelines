using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "get-iam-policy")]
public record GcloudPrivatecaPoolsGetIamPolicyOptions(
[property: PositionalArgument] string Pool,
[property: PositionalArgument] string Location
) : GcloudOptions;