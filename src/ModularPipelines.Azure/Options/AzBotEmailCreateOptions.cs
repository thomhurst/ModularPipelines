using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "email", "create")]
public record AzBotEmailCreateOptions(
[property: CommandSwitch("--email-address")] string EmailAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}