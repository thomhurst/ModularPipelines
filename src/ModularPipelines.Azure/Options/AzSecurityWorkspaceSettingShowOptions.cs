using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "workspace-setting", "show")]
public record AzSecurityWorkspaceSettingShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}