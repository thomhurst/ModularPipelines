using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "samples", "backend")]
public record GcloudSpannerSamplesBackendOptions(
[property: CliArgument] string Appname,
[property: CliOption("--instance-id")] string InstanceId
) : GcloudOptions
{
    [CliOption("--database-id")]
    public string? DatabaseId { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }
}