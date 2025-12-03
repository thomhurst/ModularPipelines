using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "custom-rollout", "list")]
public record AzProviderhubCustomRolloutListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;