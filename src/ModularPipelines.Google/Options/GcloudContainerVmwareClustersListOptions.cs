using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "list")]
public record GcloudContainerVmwareClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}