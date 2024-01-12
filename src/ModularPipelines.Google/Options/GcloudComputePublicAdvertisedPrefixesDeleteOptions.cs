using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-advertised-prefixes", "delete")]
public record GcloudComputePublicAdvertisedPrefixesDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;