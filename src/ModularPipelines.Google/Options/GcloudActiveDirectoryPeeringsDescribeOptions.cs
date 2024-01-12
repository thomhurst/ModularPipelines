using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "peerings", "describe")]
public record GcloudActiveDirectoryPeeringsDescribeOptions(
[property: PositionalArgument] string Peering
) : GcloudOptions;