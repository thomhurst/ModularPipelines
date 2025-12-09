using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "variable-group", "create")]
public record AzPipelinesVariableGroupCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--variables")] string Variables
) : AzOptions
{
    [CliFlag("--authorize")]
    public bool? Authorize { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}