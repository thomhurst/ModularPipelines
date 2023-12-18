using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "workspace-setting", "delete")]
public record AzSecurityWorkspaceSettingDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}