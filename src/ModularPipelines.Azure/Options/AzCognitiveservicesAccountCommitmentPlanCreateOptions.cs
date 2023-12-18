using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "account", "commitment-plan", "create")]
public record AzCognitiveservicesAccountCommitmentPlanCreateOptions(
[property: BooleanCommandSwitch("--auto-renew")] bool AutoRenew,
[property: CommandSwitch("--commitment-plan-name")] string CommitmentPlanName,
[property: CommandSwitch("--hosting-model")] string HostingModel,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--plan-type")] string PlanType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--current-count")]
    public int? CurrentCount { get; set; }

    [CommandSwitch("--current-tier")]
    public string? CurrentTier { get; set; }

    [CommandSwitch("--next-count")]
    public int? NextCount { get; set; }

    [CommandSwitch("--next-tier")]
    public string? NextTier { get; set; }
}