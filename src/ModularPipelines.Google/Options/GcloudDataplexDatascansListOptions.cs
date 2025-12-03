using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "list")]
public record GcloudDataplexDatascansListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}