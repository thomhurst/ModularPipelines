using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "default-rollout", "delete")]
public record AzProviderhubDefaultRolloutDeleteOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CommandSwitch("--rollout-name")]
    public string? RolloutName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}