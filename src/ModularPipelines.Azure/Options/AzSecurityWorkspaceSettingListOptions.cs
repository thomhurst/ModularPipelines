using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "workspace-setting", "list")]
public record AzSecurityWorkspaceSettingListOptions : AzOptions;