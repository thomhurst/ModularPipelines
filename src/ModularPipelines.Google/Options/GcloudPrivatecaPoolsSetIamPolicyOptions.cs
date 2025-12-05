using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "set-iam-policy")]
public record GcloudPrivatecaPoolsSetIamPolicyOptions(
[property: CliArgument] string Pool,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;