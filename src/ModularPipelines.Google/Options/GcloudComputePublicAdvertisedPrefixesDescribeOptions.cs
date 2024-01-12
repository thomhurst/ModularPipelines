using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-advertised-prefixes", "describe")]
public record GcloudComputePublicAdvertisedPrefixesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;