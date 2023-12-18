using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "show-usage")]
public record AzSqlShowUsageOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--usage")] string Usage
) : AzOptions
{
}