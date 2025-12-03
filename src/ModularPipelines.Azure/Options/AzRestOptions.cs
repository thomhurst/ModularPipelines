using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rest")]
public record AzRestOptions(
[property: CliOption("--uri")] string Uri
) : AzOptions
{
    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--headers")]
    public string? Headers { get; set; }

    [CliOption("--method")]
    public string? Method { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliFlag("--skip-authorization-header")]
    public bool? SkipAuthorizationHeader { get; set; }

    [CliOption("--uri-parameters")]
    public string? UriParameters { get; set; }
}