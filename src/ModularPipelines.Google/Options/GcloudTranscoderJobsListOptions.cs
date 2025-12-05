using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "jobs", "list")]
public record GcloudTranscoderJobsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}