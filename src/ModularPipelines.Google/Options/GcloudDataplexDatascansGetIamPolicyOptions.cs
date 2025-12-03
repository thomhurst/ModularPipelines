using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "get-iam-policy")]
public record GcloudDataplexDatascansGetIamPolicyOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions;