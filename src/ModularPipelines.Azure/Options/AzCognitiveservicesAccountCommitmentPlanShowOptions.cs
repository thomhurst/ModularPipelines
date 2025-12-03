using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognitiveservices", "account", "commitment-plan", "show")]
public record AzCognitiveservicesAccountCommitmentPlanShowOptions(
[property: CliOption("--commitment-plan-name")] string CommitmentPlanName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;