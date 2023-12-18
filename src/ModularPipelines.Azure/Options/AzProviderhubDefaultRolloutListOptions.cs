using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "default-rollout", "list")]
public record AzProviderhubDefaultRolloutListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--rollout-name")]
    public string? RolloutName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

