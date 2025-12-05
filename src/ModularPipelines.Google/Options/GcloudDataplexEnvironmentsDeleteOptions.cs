using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "delete")]
public record GcloudDataplexEnvironmentsDeleteOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}