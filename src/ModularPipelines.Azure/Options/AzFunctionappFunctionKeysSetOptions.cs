using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "function", "keys", "set")]
public record AzFunctionappFunctionKeysSetOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}