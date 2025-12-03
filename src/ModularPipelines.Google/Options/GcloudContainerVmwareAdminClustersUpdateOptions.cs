using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "admin-clusters", "update")]
public record GcloudContainerVmwareAdminClustersUpdateOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--required-platform-version")]
    public string? RequiredPlatformVersion { get; set; }
}