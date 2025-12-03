using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("blueprint", "resource-group", "update")]
public record AzBlueprintResourceGroupUpdateOptions(
[property: CliOption("--artifact-name")] string ArtifactName,
[property: CliOption("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CliOption("--depends-on")]
    public string? DependsOn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--rg-location")]
    public string? RgLocation { get; set; }

    [CliOption("--rg-name")]
    public string? RgName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}