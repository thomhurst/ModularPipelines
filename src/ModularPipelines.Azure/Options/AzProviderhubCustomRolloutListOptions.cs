using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "custom-rollout", "list")]
public record AzProviderhubCustomRolloutListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;