using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "slack", "create")]
public record AzBotSlackCreateOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--client-secret")] string ClientSecret,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--verification-token")] string VerificationToken
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CommandSwitch("--landing-page-url")]
    public string? LandingPageUrl { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}

