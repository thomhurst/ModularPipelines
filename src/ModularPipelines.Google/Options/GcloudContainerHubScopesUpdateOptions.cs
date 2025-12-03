using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "update")]
public record GcloudContainerHubScopesUpdateOptions(
[property: CliArgument] string Scope,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--update-namespace-labels")]
    public IEnumerable<KeyValue>? UpdateNamespaceLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-namespace-labels")]
    public bool? ClearNamespaceLabels { get; set; }

    [CliOption("--remove-namespace-labels")]
    public string[]? RemoveNamespaceLabels { get; set; }

    [CliOption("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [CliFlag("--remove-upgrade-soaking-overrides")]
    public bool? RemoveUpgradeSoakingOverrides { get; set; }

    [CliOption("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CliOption("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }

    [CliFlag("--reset-upstream-scope")]
    public bool? ResetUpstreamScope { get; set; }

    [CliOption("--upstream-scope")]
    public string? UpstreamScope { get; set; }
}