using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "credential", "delete")]
public record AzAdSpCredentialDeleteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--key-id")] string KeyId
) : AzOptions
{
    [CliFlag("--cert")]
    public bool? Cert { get; set; }
}