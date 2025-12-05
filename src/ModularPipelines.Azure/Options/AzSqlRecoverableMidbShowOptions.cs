using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "recoverable-midb", "show")]
public record AzSqlRecoverableMidbShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;