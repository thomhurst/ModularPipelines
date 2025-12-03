using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "keysets", "create")]
public record GcloudEdgeCacheKeysetsCreateOptions(
[property: CliArgument] string Keyset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--public-key")]
    public string[]? PublicKey { get; set; }

    [CliOption("--validation-shared-key")]
    public string[]? ValidationSharedKey { get; set; }
}