using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "keys", "list")]
public record AzStorageAccountKeysListOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--expand-key-type")]
    public string? ExpandKeyType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}