using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "job", "recurrence", "show")]
public record AzDlaJobRecurrenceShowOptions(
[property: CommandSwitch("--recurrence-identity")] string RecurrenceIdentity
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--end-date-time")]
    public string? EndDateTime { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--start-date-time")]
    public string? StartDateTime { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

