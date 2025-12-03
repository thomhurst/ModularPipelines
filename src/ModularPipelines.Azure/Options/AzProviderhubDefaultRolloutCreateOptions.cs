using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "default-rollout", "create")]
public record AzProviderhubDefaultRolloutCreateOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace,
[property: CliOption("--rollout-name")] string RolloutName
) : AzOptions
{
    [CliOption("--canary")]
    public string? Canary { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--rest-of-the-world-group-two")]
    public string? RestOfTheWorldGroupTwo { get; set; }
}