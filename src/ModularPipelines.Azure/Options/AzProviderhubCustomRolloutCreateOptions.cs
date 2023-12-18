using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "custom-rollout", "create")]
public record AzProviderhubCustomRolloutCreateOptions(
[property: CommandSwitch("--canary")] string Canary,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--rollout-name")] string RolloutName
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

