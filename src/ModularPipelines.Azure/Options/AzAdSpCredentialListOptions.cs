using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "sp", "credential", "list")]
public record AzAdSpCredentialListOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--cert")]
    public bool? Cert { get; set; }
}