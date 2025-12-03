using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "create")]
public record GcloudHealthcareDatasetsCreateOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }
}