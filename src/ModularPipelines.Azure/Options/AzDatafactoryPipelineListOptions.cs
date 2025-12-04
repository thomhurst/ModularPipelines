using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "pipeline", "list")]
public record AzDatafactoryPipelineListOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;