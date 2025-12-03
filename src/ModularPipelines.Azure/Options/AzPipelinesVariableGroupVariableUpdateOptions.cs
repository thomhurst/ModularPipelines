using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "variable-group", "variable", "update")]
public record AzPipelinesVariableGroupVariableUpdateOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--new-name")]
    public string? NewName { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--prompt-value")]
    public bool? PromptValue { get; set; }

    [CliFlag("--secret")]
    public bool? Secret { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}