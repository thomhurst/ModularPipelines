using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "clusters", "get-credentials")]
public record GcloudEdgeCloudContainerClustersGetCredentialsOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--auth-provider-cmd-path")]
    public string? AuthProviderCmdPath { get; set; }
}