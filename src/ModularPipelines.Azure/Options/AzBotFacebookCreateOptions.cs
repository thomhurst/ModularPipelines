using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "facebook", "create")]
public record AzBotFacebookCreateOptions(
[property: CliOption("--appid")] string Appid,
[property: CliOption("--name")] string Name,
[property: CliOption("--page-id")] string PageId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret")] string Secret,
[property: CliOption("--token")] string Token
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}