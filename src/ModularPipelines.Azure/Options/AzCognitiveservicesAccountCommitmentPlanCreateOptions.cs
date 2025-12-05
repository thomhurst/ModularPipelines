using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "account", "commitment-plan", "create")]
public record AzCognitiveservicesAccountCommitmentPlanCreateOptions(
[property: CliFlag("--auto-renew")] bool AutoRenew,
[property: CliOption("--commitment-plan-name")] string CommitmentPlanName,
[property: CliOption("--hosting-model")] string HostingModel,
[property: CliOption("--name")] string Name,
[property: CliOption("--plan-type")] string PlanType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--current-count")]
    public int? CurrentCount { get; set; }

    [CliOption("--current-tier")]
    public string? CurrentTier { get; set; }

    [CliOption("--next-count")]
    public int? NextCount { get; set; }

    [CliOption("--next-tier")]
    public string? NextTier { get; set; }
}