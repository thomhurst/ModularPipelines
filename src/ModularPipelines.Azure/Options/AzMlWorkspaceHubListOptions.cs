using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace-hub", "list")]
public record AzMlWorkspaceHubListOptions : AzOptions
{
    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}