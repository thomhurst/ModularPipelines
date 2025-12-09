using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blueprint", "artifact", "list")]
public record AzBlueprintArtifactListOptions(
[property: CliOption("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}