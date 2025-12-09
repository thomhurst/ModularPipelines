using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "resource-group", "remove")]
public record AzBlueprintResourceGroupRemoveOptions(
[property: CliOption("--artifact-name")] string ArtifactName,
[property: CliOption("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}