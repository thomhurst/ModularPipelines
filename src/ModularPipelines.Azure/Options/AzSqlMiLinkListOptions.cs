using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "link", "list")]
public record AzSqlMiLinkListOptions(
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;