using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "online-deployment", "list")]
public record AzMlOnlineDeploymentListOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--local")]
    public bool? Local { get; set; }
}