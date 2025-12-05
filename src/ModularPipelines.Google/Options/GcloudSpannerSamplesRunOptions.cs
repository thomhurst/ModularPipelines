using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "samples", "run")]
public record GcloudSpannerSamplesRunOptions(
[property: CliArgument] string Appname,
[property: CliOption("--instance-id")] string InstanceId
) : GcloudOptions
{
    [CliFlag("--cleanup")]
    public bool? Cleanup { get; set; }

    [CliOption("--database-id")]
    public string? DatabaseId { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliFlag("--skip-init")]
    public bool? SkipInit { get; set; }
}