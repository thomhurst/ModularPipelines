using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-advertised-prefixes", "describe")]
public record GcloudComputePublicAdvertisedPrefixesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;