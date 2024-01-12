using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "operations", "list")]
public record GcloudContainerAttachedOperationsListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}