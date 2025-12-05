using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server-arc", "extension", "feature-flag", "delete")]
public record AzSqlServerArcExtensionFeatureFlagDeleteOptions(
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;