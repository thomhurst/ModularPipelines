using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "dataset", "create")]
public record AzDatafactoryDatasetCreateOptions(
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--properties")] string Properties,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}