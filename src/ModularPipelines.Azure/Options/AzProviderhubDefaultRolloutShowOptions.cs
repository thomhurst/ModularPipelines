using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "default-rollout", "show")]
public record AzProviderhubDefaultRolloutShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CliOption("--rollout-name")]
    public string? RolloutName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}