using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "facebook", "create")]
public record AzBotFacebookCreateOptions(
[property: CommandSwitch("--appid")] string Appid,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--page-id")] string PageId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret")] string Secret,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}