using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-action-workspace", "create")]
public record AzSecurityAutomationActionWorkspaceCreateOptions(
[property: CommandSwitch("--workspace-resource-id")] string WorkspaceResourceId
) : AzOptions
{
}