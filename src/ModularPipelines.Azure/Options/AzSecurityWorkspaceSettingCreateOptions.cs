using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "workspace-setting", "create")]
public record AzSecurityWorkspaceSettingCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--target-workspace")] string TargetWorkspace
) : AzOptions;