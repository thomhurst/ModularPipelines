using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "peerings", "delete")]
public record GcloudActiveDirectoryPeeringsDeleteOptions(
[property: CliArgument] string Peering
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}