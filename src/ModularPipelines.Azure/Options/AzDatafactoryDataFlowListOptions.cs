using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "data-flow", "list")]
public record AzDatafactoryDataFlowListOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;