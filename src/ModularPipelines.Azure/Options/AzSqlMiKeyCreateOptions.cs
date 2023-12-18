using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "key", "create")]
public record AzSqlMiKeyCreateOptions(
[property: CommandSwitch("--kid")] string Kid,
[property: CommandSwitch("--managed-instance")] string ManagedInstance,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;