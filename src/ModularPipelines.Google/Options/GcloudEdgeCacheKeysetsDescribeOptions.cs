using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "keysets", "describe")]
public record GcloudEdgeCacheKeysetsDescribeOptions(
[property: PositionalArgument] string Keyset,
[property: PositionalArgument] string Location
) : GcloudOptions;