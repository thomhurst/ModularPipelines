using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "clusters", "list")]
public record GcloudContainerAwsClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}