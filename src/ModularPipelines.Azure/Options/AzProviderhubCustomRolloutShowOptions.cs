using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "custom-rollout", "show")]
public record AzProviderhubCustomRolloutShowOptions : AzOptions
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