using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "export", "bak")]
public record GcloudSqlExportBakOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri,
[property: CommandSwitch("--database")] string[] Database
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--bak-type")]
    public string? BakType { get; set; }

    [BooleanCommandSwitch("--differential-base")]
    public bool? DifferentialBase { get; set; }

    [CommandSwitch("--stripe_count")]
    public string? StripeCount { get; set; }

    [CommandSwitch("--[no-]striped")]
    public string[]? NoStriped { get; set; }
}