using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "account", "commitment-plan", "delete")]
public record AzCognitiveservicesAccountCommitmentPlanDeleteOptions(
[property: CommandSwitch("--commitment-plan-name")] string CommitmentPlanName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;