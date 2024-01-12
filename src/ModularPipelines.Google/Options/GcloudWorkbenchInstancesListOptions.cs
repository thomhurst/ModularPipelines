using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "list")]
public record GcloudWorkbenchInstancesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}