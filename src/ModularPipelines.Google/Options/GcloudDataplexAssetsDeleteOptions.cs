using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "delete")]
public record GcloudDataplexAssetsDeleteOptions(
[property: CliArgument] string Asset,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}