using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "clusters", "get-credentials")]
public record GcloudContainerAwsClustersGetCredentialsOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }
}