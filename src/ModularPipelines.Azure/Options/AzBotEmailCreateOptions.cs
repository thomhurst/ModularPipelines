using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "email", "create")]
public record AzBotEmailCreateOptions(
[property: CliOption("--email-address")] string EmailAddress,
[property: CliOption("--name")] string Name,
[property: CliOption("--password")] string Password,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}