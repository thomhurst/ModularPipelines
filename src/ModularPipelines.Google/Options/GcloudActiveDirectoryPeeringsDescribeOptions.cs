using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "peerings", "describe")]
public record GcloudActiveDirectoryPeeringsDescribeOptions(
[property: CliArgument] string Peering
) : GcloudOptions;