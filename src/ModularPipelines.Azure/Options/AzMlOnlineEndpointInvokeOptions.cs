using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "online-endpoint", "invoke")]
public record AzMlOnlineEndpointInvokeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliOption("--request-file")]
    public string? RequestFile { get; set; }
}