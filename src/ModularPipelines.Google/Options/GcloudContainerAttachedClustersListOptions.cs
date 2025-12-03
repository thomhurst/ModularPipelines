using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "list")]
public record GcloudContainerAttachedClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}