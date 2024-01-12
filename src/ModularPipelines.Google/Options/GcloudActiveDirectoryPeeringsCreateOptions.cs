using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "peerings", "create")]
public record GcloudActiveDirectoryPeeringsCreateOptions(
[property: PositionalArgument] string Peering,
[property: CommandSwitch("--authorized-network")] string AuthorizedNetwork,
[property: CommandSwitch("--domain")] string Domain
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}