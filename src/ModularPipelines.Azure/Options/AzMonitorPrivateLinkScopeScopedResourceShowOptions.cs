using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "private-link-scope", "scoped-resource", "show")]
public record AzMonitorPrivateLinkScopeScopedResourceShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope-name")]
    public string? ScopeName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}