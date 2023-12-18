using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace", "outbound-rule", "show")]
public record AzMlWorkspaceOutboundRuleShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}