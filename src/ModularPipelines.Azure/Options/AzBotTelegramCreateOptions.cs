using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "telegram", "create")]
public record AzBotTelegramCreateOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [BooleanCommandSwitch("--is-validated")]
    public bool? IsValidated { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}