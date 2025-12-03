using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instance-configs", "delete")]
public record GcloudSpannerInstanceConfigsDeleteOptions(
[property: CliArgument] string InstanceConfig
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}