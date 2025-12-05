using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "versions", "list")]
public record GcloudComputeTpusVersionsListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}