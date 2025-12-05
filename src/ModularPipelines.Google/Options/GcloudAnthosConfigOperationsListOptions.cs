using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "operations", "list")]
public record GcloudAnthosConfigOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}