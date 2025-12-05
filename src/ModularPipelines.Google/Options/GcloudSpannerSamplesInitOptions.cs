using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "samples", "init")]
public record GcloudSpannerSamplesInitOptions(
[property: CliArgument] string Appname,
[property: CliOption("--instance-id")] string InstanceId
) : GcloudOptions
{
    [CliOption("--database-id")]
    public string? DatabaseId { get; set; }
}