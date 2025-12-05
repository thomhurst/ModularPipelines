using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "templates", "list")]
public record GcloudTranscoderTemplatesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}