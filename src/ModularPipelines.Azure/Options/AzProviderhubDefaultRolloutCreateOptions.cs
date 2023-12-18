using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "default-rollout", "create")]
public record AzProviderhubDefaultRolloutCreateOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--rollout-name")] string RolloutName
) : AzOptions
{
    [CommandSwitch("--canary")]
    public string? Canary { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--rest-of-the-world-group-two")]
    public string? RestOfTheWorldGroupTwo { get; set; }
}

