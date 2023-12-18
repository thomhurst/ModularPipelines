using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("graph", "query")]
public record AzGraphQueryOptions(
[property: CommandSwitch("--graph-query")] string GraphQuery
) : AzOptions
{
    [BooleanCommandSwitch("--allow-partial-scopes")]
    public bool? AllowPartialScopes { get; set; }

    [CommandSwitch("--first")]
    public string? First { get; set; }

    [CommandSwitch("--management-groups")]
    public string? ManagementGroups { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--subscriptions")]
    public string? Subscriptions { get; set; }
}