using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maps", "account", "keys", "renew")]
public record AzMapsAccountKeysRenewOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;