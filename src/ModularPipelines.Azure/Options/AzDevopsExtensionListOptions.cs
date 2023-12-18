using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "extension", "list")]
public record AzDevopsExtensionListOptions : AzOptions
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