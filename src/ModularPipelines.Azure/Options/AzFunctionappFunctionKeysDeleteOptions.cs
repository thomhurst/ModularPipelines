using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "function", "keys", "delete")]
public record AzFunctionappFunctionKeysDeleteOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--function-name")]
    public string? FunctionName { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}