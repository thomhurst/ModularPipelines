using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "get-iam-policy")]
public record GcloudPrivatecaPoolsGetIamPolicyOptions(
[property: CliArgument] string Pool,
[property: CliArgument] string Location
) : GcloudOptions;