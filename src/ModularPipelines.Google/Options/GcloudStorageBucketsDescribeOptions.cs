using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "describe")]
public record GcloudStorageBucketsDescribeOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--raw")]
    public bool? Raw { get; set; }
}