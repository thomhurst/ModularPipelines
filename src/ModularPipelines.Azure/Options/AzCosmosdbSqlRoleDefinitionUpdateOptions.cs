using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "role", "definition", "update")]
public record AzCosmosdbSqlRoleDefinitionUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}