using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "workspace-setting", "list")]
public record AzSecurityWorkspaceSettingListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}