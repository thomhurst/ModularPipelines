using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "dev-box", "list")]
public record AzDevcenterDevDevBoxListOptions : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}