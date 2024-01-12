using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "keysets", "export")]
public record GcloudEdgeCacheKeysetsExportOptions(
[property: PositionalArgument] string Keyset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}