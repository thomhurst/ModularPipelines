using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "online-deployment", "get-logs")]
public record AzMlOnlineDeploymentGetLogsOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--lines")]
    public string? Lines { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }
}