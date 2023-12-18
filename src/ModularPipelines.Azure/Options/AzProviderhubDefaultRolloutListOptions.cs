using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "default-rollout", "list")]
public record AzProviderhubDefaultRolloutListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;