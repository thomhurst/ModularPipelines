using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "list")]
public record GcloudContainerBareMetalClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}