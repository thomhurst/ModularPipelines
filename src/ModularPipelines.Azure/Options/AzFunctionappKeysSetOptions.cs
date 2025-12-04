using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "keys", "set")]
public record AzFunctionappKeysSetOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--key-type")] string KeyType,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}