using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "allowed_connections", "list")]
public record AzSecurityAllowed_connectionsListOptions : AzOptions;