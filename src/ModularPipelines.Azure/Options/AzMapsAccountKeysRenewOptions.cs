using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maps", "account", "keys", "renew")]
public record AzMapsAccountKeysRenewOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}