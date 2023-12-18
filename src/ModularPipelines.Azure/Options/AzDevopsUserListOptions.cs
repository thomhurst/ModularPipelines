using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "user", "list")]
public record AzDevopsUserListOptions(
[property: CommandSwitch("--user")] string User
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

