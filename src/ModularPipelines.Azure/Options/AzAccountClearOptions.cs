using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "clear")]
public record AzAccountClearOptions(
[property: CommandSwitch("--enrollment-account-name")] int EnrollmentAccountName,
[property: CommandSwitch("--offer-type")] string OfferType
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--owner-object-id")]
    public string? OwnerObjectId { get; set; }

    [CommandSwitch("--owner-spn")]
    public string? OwnerSpn { get; set; }

    [CommandSwitch("--owner-upn")]
    public string? OwnerUpn { get; set; }
}