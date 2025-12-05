using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "artifact", "show")]
public record AzBlueprintArtifactShowOptions(
[property: CliOption("--blueprint-name")] string BlueprintName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}