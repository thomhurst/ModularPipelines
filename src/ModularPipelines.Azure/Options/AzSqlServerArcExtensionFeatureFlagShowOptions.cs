using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "server-arc", "extension", "feature-flag", "show")]
public record AzSqlServerArcExtensionFeatureFlagShowOptions(
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}