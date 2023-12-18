using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "dev-box", "delete")]
public record AzDevcenterDevDevBoxDeleteOptions(
[property: CommandSwitch("--dev-box-name")] string DevBoxName,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}