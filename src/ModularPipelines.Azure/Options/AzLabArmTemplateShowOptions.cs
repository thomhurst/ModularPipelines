using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lab", "arm-template", "show")]
public record AzLabArmTemplateShowOptions(
[property: CliOption("--artifact-source-name")] string ArtifactSourceName,
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--export-parameters")]
    public bool? ExportParameters { get; set; }
}