using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "data-flow", "create")]
public record AzDatafactoryDataFlowCreateOptions(
[property: CliOption("--data-flow-name")] string DataFlowName,
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--flow-type")] string FlowType,
[property: CliOption("--properties")] string Properties,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}