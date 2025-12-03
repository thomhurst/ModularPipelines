using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "peerings", "list")]
public record GcloudActiveDirectoryPeeringsListOptions : GcloudOptions;