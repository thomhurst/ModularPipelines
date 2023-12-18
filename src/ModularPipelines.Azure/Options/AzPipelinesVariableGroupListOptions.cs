using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable-group", "list")]
public record AzPipelinesVariableGroupListOptions : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--query-order")]
    public string? QueryOrder { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}