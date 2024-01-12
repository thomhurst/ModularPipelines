using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "admin-clusters", "query-version-config")]
public record GcloudContainerBareMetalAdminClustersQueryVersionConfigOptions : GcloudOptions
{
    [CommandSwitch("--admin-cluster")]
    public string? AdminCluster { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}