using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "admin-clusters", "update")]
public record GcloudContainerVmwareAdminClustersUpdateOptions(
[property: PositionalArgument] string AdminCluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--required-platform-version")]
    public string? RequiredPlatformVersion { get; set; }
}