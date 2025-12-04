using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "pipeline", "create")]
public record AzDatafactoryPipelineCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pipeline")] string Pipeline,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}