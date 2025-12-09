using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "recoverable-midb", "list")]
public record AzSqlRecoverableMidbListOptions(
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;