using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "extension", "list")]
public record AzDevopsExtensionListOptions(
[property: CommandSwitch("--search-query")] string SearchQuery
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--include-built-in")]
    public bool? IncludeBuiltIn { get; set; }

    [BooleanCommandSwitch("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}

