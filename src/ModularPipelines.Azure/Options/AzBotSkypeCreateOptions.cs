using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "skype", "create")]
public record AzBotSkypeCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliOption("--calling-web-hook")]
    public string? CallingWebHook { get; set; }

    [CliFlag("--enable-calling")]
    public bool? EnableCalling { get; set; }

    [CliFlag("--enable-groups")]
    public bool? EnableGroups { get; set; }

    [CliFlag("--enable-media-cards")]
    public bool? EnableMediaCards { get; set; }

    [CliFlag("--enable-messaging")]
    public bool? EnableMessaging { get; set; }

    [CliFlag("--enable-screen-sharing")]
    public bool? EnableScreenSharing { get; set; }

    [CliFlag("--enable-video")]
    public bool? EnableVideo { get; set; }

    [CliOption("--groups-mode")]
    public string? GroupsMode { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}