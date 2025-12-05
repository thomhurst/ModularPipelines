using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "list")]
public record GcloudDataplexLakesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}