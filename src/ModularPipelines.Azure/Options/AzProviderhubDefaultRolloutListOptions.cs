using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "default-rollout", "list")]
public record AzProviderhubDefaultRolloutListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;