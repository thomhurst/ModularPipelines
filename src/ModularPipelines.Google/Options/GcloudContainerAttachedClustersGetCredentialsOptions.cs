using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "get-credentials")]
public record GcloudContainerAttachedClustersGetCredentialsOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;