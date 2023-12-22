using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "commitment-tier", "list")]
public record AzCognitiveservicesCommitmentTierListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;