using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("blueprint", "artifact", "template", "update")]
public record AzBlueprintArtifactTemplateUpdateOptions(
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

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--resource-group-art")]
    public string? ResourceGroupArt { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}