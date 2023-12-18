using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "pipeline", "create")]
public record AzDatafactoryPipelineCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pipeline")] string Pipeline,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}