using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "update")]
public record GcloudContainerFleetScopesUpdateOptions(
[property: PositionalArgument] string Scope,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--update-namespace-labels")]
    public IEnumerable<KeyValue>? UpdateNamespaceLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-namespace-labels")]
    public bool? ClearNamespaceLabels { get; set; }

    [CommandSwitch("--remove-namespace-labels")]
    public string[]? RemoveNamespaceLabels { get; set; }

    [CommandSwitch("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [BooleanCommandSwitch("--remove-upgrade-soaking-overrides")]
    public bool? RemoveUpgradeSoakingOverrides { get; set; }

    [CommandSwitch("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CommandSwitch("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }

    [BooleanCommandSwitch("--reset-upstream-scope")]
    public bool? ResetUpstreamScope { get; set; }

    [CommandSwitch("--upstream-scope")]
    public string? UpstreamScope { get; set; }
}