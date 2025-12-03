using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "ddl", "update")]
public record GcloudSpannerDatabasesDdlUpdateOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--ddl")]
    public string? Ddl { get; set; }

    [CliOption("--ddl-file")]
    public string? DdlFile { get; set; }
}