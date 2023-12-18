using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server-arc", "extension", "feature-flag", "set")]
public record AzSqlServerArcExtensionFeatureFlagSetOptions(
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
}

