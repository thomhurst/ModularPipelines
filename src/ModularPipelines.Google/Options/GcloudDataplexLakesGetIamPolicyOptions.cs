using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "get-iam-policy")]
public record GcloudDataplexLakesGetIamPolicyOptions(
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;