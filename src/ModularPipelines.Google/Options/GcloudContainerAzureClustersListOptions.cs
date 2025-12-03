using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clusters", "list")]
public record GcloudContainerAzureClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}