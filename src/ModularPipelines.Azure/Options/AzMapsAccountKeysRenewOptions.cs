using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maps", "account", "keys", "renew")]
public record AzMapsAccountKeysRenewOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--key")] string Key,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;