using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "encryption", "rotate-key")]
public record AzAcrEncryptionRotateKeyOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}