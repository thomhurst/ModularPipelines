using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "list-usages")]
public record AzSqlListUsagesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;