using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "msteams", "create")]
public record AzBotMsteamsCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CommandSwitch("--calling-web-hook")]
    public string? CallingWebHook { get; set; }

    [BooleanCommandSwitch("--enable-calling")]
    public bool? EnableCalling { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}