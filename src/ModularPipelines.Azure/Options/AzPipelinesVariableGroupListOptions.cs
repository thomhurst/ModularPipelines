using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "variable-group", "list")]
public record AzPipelinesVariableGroupListOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--query-order")]
    public string? QueryOrder { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}