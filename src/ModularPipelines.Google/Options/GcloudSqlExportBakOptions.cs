using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "export", "bak")]
public record GcloudSqlExportBakOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri,
[property: CliOption("--database")] string[] Database
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--bak-type")]
    public string? BakType { get; set; }

    [CliFlag("--differential-base")]
    public bool? DifferentialBase { get; set; }

    [CliOption("--stripe_count")]
    public string? StripeCount { get; set; }

    [CliOption("--[no-]striped")]
    public string[]? NoStriped { get; set; }
}