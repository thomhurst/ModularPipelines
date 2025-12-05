using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "zones", "set-iam-policy")]
public record GcloudDataplexZonesSetIamPolicyOptions(
[property: CliArgument] string Zone,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;