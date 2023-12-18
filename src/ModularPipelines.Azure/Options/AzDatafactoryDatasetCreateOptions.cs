using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "dataset", "create")]
public record AzDatafactoryDatasetCreateOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--properties")] string Properties,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}