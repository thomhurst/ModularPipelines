using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "function", "keys", "delete")]
public record AzFunctionappFunctionKeysDeleteOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--function-name")]
    public string? FunctionName { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}