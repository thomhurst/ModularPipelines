using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "clusters", "get-credentials")]
public record GcloudContainerAwsClustersGetCredentialsOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }
}