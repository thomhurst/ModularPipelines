using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "admin-clusters", "list")]
public record GcloudContainerVmwareAdminClustersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}