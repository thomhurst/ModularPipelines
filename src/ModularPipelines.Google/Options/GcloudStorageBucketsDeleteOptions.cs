using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "delete")]
public record GcloudStorageBucketsDeleteOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }
}