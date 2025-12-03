using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mesh", "generate", "armtemplate")]
public record AzMeshGenerateArmtemplateOptions(
[property: CliOption("--input-yaml-files")] string InputYamlFiles
) : AzOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }
}