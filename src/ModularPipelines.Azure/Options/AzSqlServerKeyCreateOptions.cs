using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "key", "create")]
public record AzSqlServerKeyCreateOptions(
[property: CommandSwitch("--kid")] string Kid,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions;