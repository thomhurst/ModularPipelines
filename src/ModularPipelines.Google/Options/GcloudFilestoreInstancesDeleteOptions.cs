using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "delete")]
public record GcloudFilestoreInstancesDeleteOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}