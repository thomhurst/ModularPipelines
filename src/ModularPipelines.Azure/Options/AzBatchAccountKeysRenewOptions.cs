using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "account", "keys", "renew")]
public record AzBatchAccountKeysRenewOptions : AzOptions
{
    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}