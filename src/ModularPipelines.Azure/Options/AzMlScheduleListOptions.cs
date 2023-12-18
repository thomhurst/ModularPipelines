using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "schedule", "list")]
public record AzMlScheduleListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--disabled-only")]
    public bool? DisabledOnly { get; set; }

    [BooleanCommandSwitch("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}