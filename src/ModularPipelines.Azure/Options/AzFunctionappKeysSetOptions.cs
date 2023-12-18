using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "keys", "set")]
public record AzFunctionappKeysSetOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--key-type")] string KeyType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}