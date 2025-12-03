using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "credential", "list")]
public record AzAdAppCredentialListOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--cert")]
    public bool? Cert { get; set; }
}