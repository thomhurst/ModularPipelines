using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "function", "keys", "set")]
public record AzFunctionappFunctionKeysSetOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}