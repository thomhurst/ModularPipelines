using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi", "key", "create")]
public record AzSqlMiKeyCreateOptions(
[property: CliOption("--kid")] string Kid,
[property: CliOption("--managed-instance")] string ManagedInstance,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;