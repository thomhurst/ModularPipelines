using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lab", "environment", "create")]
public record AzLabEnvironmentCreateOptions(
[property: CliOption("--arm-template")] string ArmTemplate,
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--artifact-source-name")]
    public string? ArtifactSourceName { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}