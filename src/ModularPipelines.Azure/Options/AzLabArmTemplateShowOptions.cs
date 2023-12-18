using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lab", "arm-template", "show")]
public record AzLabArmTemplateShowOptions(
[property: CommandSwitch("--artifact-source-name")] string ArtifactSourceName,
[property: CommandSwitch("--lab-name")] string LabName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--export-parameters")]
    public bool? ExportParameters { get; set; }
}