using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bot", "slack", "create")]
public record AzBotSlackCreateOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--verification-token")] string VerificationToken
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliOption("--landing-page-url")]
    public string? LandingPageUrl { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}