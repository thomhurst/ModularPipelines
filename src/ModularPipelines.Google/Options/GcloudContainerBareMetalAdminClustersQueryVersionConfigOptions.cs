using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "query-version-config")]
public record GcloudContainerBareMetalAdminClustersQueryVersionConfigOptions : GcloudOptions
{
    [CliOption("--admin-cluster")]
    public string? AdminCluster { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}