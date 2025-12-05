using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "set-iam-policy")]
public record GcloudDataplexDatascansSetIamPolicyOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;