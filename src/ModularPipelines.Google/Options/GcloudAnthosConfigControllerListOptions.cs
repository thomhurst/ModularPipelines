using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "list")]
public record GcloudAnthosConfigControllerListOptions : GcloudOptions
{
    [CliFlag("--full-name")]
    public bool? FullName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}