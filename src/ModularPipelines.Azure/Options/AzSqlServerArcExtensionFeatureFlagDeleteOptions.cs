using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server-arc", "extension", "feature-flag", "delete")]
public record AzSqlServerArcExtensionFeatureFlagDeleteOptions(
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;