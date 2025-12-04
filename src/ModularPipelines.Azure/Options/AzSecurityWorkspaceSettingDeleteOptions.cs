using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "workspace-setting", "delete")]
public record AzSecurityWorkspaceSettingDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions;