using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "variable-group", "delete")]
public record AzPipelinesVariableGroupDeleteOptions(
[property: CliOption("--group-id")] string GroupId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}