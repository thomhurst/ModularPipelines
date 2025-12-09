using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "telegram", "create")]
public record AzBotTelegramCreateOptions(
[property: CliOption("--access-token")] string AccessToken,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliFlag("--is-validated")]
    public bool? IsValidated { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}