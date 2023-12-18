using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "link", "list")]
public record AzResourceLinkListOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}