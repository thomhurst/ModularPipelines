using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "action-group", "create")]
public record AzMonitorActionGroupCreateOptions(
[property: CommandSwitch("--action-group-name")] string ActionGroupName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--group-short-name")]
    public string? GroupShortName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}