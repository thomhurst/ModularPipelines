using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "custom-rollout", "create")]
public record AzProviderhubCustomRolloutCreateOptions(
[property: CommandSwitch("--canary")] string Canary,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--rollout-name")] string RolloutName
) : AzOptions;