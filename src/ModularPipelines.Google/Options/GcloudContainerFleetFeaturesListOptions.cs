using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "features", "list")]
public record GcloudContainerFleetFeaturesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}