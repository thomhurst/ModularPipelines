using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "custom-rollout", "create")]
public record AzProviderhubCustomRolloutCreateOptions(
[property: CliOption("--canary")] string Canary,
[property: CliOption("--provider-namespace")] string ProviderNamespace,
[property: CliOption("--rollout-name")] string RolloutName
) : AzOptions;