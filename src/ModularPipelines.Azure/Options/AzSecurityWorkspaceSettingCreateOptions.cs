using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "workspace-setting", "create")]
public record AzSecurityWorkspaceSettingCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--target-workspace")] string TargetWorkspace
) : AzOptions;