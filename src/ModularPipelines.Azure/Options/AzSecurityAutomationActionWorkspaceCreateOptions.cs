using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "automation-action-workspace", "create")]
public record AzSecurityAutomationActionWorkspaceCreateOptions(
[property: CliOption("--workspace-resource-id")] string WorkspaceResourceId
) : AzOptions;