using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "skype", "create")]
public record AzBotSkypeCreateOptions(
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

    [BooleanCommandSwitch("--enable-groups")]
    public bool? EnableGroups { get; set; }

    [BooleanCommandSwitch("--enable-media-cards")]
    public bool? EnableMediaCards { get; set; }

    [BooleanCommandSwitch("--enable-messaging")]
    public bool? EnableMessaging { get; set; }

    [BooleanCommandSwitch("--enable-screen-sharing")]
    public bool? EnableScreenSharing { get; set; }

    [BooleanCommandSwitch("--enable-video")]
    public bool? EnableVideo { get; set; }

    [CommandSwitch("--groups-mode")]
    public string? GroupsMode { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}