using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mesh", "generate", "armtemplate")]
public record AzMeshGenerateArmtemplateOptions(
[property: CommandSwitch("--input-yaml-files")] string InputYamlFiles
) : AzOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }
}

