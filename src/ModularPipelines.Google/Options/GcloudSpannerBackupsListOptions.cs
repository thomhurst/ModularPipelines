using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "list")]
public record GcloudSpannerBackupsListOptions : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }
}