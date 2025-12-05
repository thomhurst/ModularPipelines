using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "regions", "list")]
public record GcloudFunctionsRegionsListOptions : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}