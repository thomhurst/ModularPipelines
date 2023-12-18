using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "list")]
public record AzTagListOptions : AzOptions
{
    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }
}