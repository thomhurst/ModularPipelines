using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "credential", "delete")]
public record AzAdAppCredentialDeleteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--key-id")] string KeyId
) : AzOptions
{
    [CliFlag("--cert")]
    public bool? Cert { get; set; }
}