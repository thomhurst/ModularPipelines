using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "peerings", "create")]
public record GcloudActiveDirectoryPeeringsCreateOptions(
[property: CliArgument] string Peering,
[property: CliOption("--authorized-network")] string AuthorizedNetwork,
[property: CliOption("--domain")] string Domain
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}