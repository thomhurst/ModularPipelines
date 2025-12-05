using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "update-profile")]
public record AwsCustomerProfilesUpdateProfileOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--profile-id")] string ProfileId
) : AwsOptions
{
    [CliOption("--additional-information")]
    public string? AdditionalInformation { get; set; }

    [CliOption("--account-number")]
    public string? AccountNumber { get; set; }

    [CliOption("--party-type")]
    public string? PartyType { get; set; }

    [CliOption("--business-name")]
    public string? BusinessName { get; set; }

    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--middle-name")]
    public string? MiddleName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--birth-date")]
    public string? BirthDate { get; set; }

    [CliOption("--gender")]
    public string? Gender { get; set; }

    [CliOption("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CliOption("--mobile-phone-number")]
    public string? MobilePhoneNumber { get; set; }

    [CliOption("--home-phone-number")]
    public string? HomePhoneNumber { get; set; }

    [CliOption("--business-phone-number")]
    public string? BusinessPhoneNumber { get; set; }

    [CliOption("--email-address")]
    public string? EmailAddress { get; set; }

    [CliOption("--personal-email-address")]
    public string? PersonalEmailAddress { get; set; }

    [CliOption("--business-email-address")]
    public string? BusinessEmailAddress { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CliOption("--mailing-address")]
    public string? MailingAddress { get; set; }

    [CliOption("--billing-address")]
    public string? BillingAddress { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--party-type-string")]
    public string? PartyTypeString { get; set; }

    [CliOption("--gender-string")]
    public string? GenderString { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}