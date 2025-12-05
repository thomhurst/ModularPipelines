using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "private-link-scope", "scoped-resource", "create")]
public record AzMonitorPrivateLinkScopeScopedResourceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scope-name")] string ScopeName
) : AzOptions
{
    [CliOption("--linked-resource")]
    public string? LinkedResource { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}