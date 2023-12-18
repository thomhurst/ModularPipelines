using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "data-flow", "create")]
public record AzDatafactoryDataFlowCreateOptions(
[property: CommandSwitch("--data-flow-name")] string DataFlowName,
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--flow-type")] string FlowType,
[property: CommandSwitch("--properties")] string Properties,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}