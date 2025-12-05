using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "keysets", "export")]
public record GcloudEdgeCacheKeysetsExportOptions(
[property: CliArgument] string Keyset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}