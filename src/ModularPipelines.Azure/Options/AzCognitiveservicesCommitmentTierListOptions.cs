using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "commitment-tier", "list")]
public record AzCognitiveservicesCommitmentTierListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;