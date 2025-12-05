using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "call")]
public record GcloudFunctionsCallOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }

    [CliOption("--cloud-event")]
    public string? CloudEvent { get; set; }

    [CliOption("--data")]
    public string? Data { get; set; }
}