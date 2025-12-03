using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "update")]
public record GcloudSpannerDatabasesUpdateOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--[no-]enable-drop-protection")]
    public string[]? NoEnableDropProtection { get; set; }
}