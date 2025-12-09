using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "workspace-setting", "show")]
public record AzSecurityWorkspaceSettingShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;