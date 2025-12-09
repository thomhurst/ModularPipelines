using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("graph", "query")]
public record AzGraphQueryOptions(
[property: CliOption("--graph-query")] string GraphQuery
) : AzOptions
{
    [CliFlag("--allow-partial-scopes")]
    public bool? AllowPartialScopes { get; set; }

    [CliOption("--first")]
    public string? First { get; set; }

    [CliOption("--management-groups")]
    public string? ManagementGroups { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--subscriptions")]
    public string? Subscriptions { get; set; }
}