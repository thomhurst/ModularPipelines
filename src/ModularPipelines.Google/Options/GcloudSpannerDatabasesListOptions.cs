using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "list")]
public record GcloudSpannerDatabasesListOptions : GcloudOptions
{
    [CliOption("--instance")]
    public string? Instance { get; set; }
}