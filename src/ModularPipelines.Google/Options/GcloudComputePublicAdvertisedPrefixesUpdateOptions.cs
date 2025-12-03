using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-advertised-prefixes", "update")]
public record GcloudComputePublicAdvertisedPrefixesUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--status")] string Status
) : GcloudOptions;