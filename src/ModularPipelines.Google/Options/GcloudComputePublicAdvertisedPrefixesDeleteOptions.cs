using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-advertised-prefixes", "delete")]
public record GcloudComputePublicAdvertisedPrefixesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;