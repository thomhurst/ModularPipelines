using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-action-workspace", "create")]
public record AzSecurityAutomationActionWorkspaceCreateOptions(
[property: CommandSwitch("--workspace-resource-id")] string WorkspaceResourceId
) : AzOptions;