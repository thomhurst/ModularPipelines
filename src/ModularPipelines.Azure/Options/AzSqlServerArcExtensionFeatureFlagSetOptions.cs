using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "server-arc", "extension", "feature-flag", "set")]
public record AzSqlServerArcExtensionFeatureFlagSetOptions(
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--value")] string Value
) : AzOptions;