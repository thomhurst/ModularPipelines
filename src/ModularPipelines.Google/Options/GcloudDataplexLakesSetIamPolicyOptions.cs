using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "set-iam-policy")]
public record GcloudDataplexLakesSetIamPolicyOptions(
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;