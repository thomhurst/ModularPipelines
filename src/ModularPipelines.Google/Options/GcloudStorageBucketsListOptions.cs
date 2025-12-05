using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "list")]
public record GcloudStorageBucketsListOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--raw")]
    public bool? Raw { get; set; }
}