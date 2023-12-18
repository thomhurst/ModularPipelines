using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "data-flow", "list")]
public record AzDatafactoryDataFlowListOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;