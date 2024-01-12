using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "keysets", "update")]
public record GcloudEdgeCacheKeysetsUpdateOptions(
[property: PositionalArgument] string Keyset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--public-key")]
    public string[]? PublicKey { get; set; }

    [CommandSwitch("--validation-shared-key")]
    public string[]? ValidationSharedKey { get; set; }
}