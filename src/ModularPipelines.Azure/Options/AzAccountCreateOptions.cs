using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "create")]
public record AzAccountCreateOptions(
[property: CliOption("--enrollment-account-name")] int EnrollmentAccountName,
[property: CliOption("--offer-type")] string OfferType
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--owner-object-id")]
    public string? OwnerObjectId { get; set; }

    [CliOption("--owner-spn")]
    public string? OwnerSpn { get; set; }

    [CliOption("--owner-upn")]
    public string? OwnerUpn { get; set; }
}