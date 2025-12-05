using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "zones", "get-iam-policy")]
public record GcloudDataplexZonesGetIamPolicyOptions(
[property: CliArgument] string Zone,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;