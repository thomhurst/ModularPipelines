using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "keysets", "describe")]
public record GcloudEdgeCacheKeysetsDescribeOptions(
[property: CliArgument] string Keyset,
[property: CliArgument] string Location
) : GcloudOptions;