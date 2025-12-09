using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "sms", "create")]
public record AzBotSmsCreateOptions(
[property: CliOption("--account-sid")] int AccountSid,
[property: CliOption("--auth-token")] string AuthToken,
[property: CliOption("--name")] string Name,
[property: CliOption("--phone")] string Phone,
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