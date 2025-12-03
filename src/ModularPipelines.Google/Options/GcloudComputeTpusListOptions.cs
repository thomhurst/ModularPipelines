using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "list")]
public record GcloudComputeTpusListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}