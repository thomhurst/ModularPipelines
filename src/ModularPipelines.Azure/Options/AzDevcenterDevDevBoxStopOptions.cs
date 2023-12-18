using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "dev-box", "stop")]
public record AzDevcenterDevDevBoxStopOptions(
[property: CommandSwitch("--dev-box-name")] string DevBoxName,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--hibernate")]
    public bool? Hibernate { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}

