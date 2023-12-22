using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "workspace-setting", "delete")]
public record AzSecurityWorkspaceSettingDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;