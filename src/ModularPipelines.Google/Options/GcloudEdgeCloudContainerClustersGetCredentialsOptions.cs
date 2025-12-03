using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "get-credentials")]
public record GcloudEdgeCloudContainerClustersGetCredentialsOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--auth-provider-cmd-path")]
    public string? AuthProviderCmdPath { get; set; }
}