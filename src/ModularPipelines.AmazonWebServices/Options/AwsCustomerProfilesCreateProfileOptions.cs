using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "create-profile")]
public record AwsCustomerProfilesCreateProfileOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--account-number")]
    public string? AccountNumber { get; set; }

    [CommandSwitch("--additional-information")]
    public string? AdditionalInformation { get; set; }

    [CommandSwitch("--party-type")]
    public string? PartyType { get; set; }

    [CommandSwitch("--business-name")]
    public string? BusinessName { get; set; }

    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--middle-name")]
    public string? MiddleName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--birth-date")]
    public string? BirthDate { get; set; }

    [CommandSwitch("--gender")]
    public string? Gender { get; set; }

    [CommandSwitch("--phone-number")]
    public string? PhoneNumber { get; set; }

    [CommandSwitch("--mobile-phone-number")]
    public string? MobilePhoneNumber { get; set; }

    [CommandSwitch("--home-phone-number")]
    public string? HomePhoneNumber { get; set; }

    [CommandSwitch("--business-phone-number")]
    public string? BusinessPhoneNumber { get; set; }

    [CommandSwitch("--email-address")]
    public string? EmailAddress { get; set; }

    [CommandSwitch("--personal-email-address")]
    public string? PersonalEmailAddress { get; set; }

    [CommandSwitch("--business-email-address")]
    public string? BusinessEmailAddress { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CommandSwitch("--mailing-address")]
    public string? MailingAddress { get; set; }

    [CommandSwitch("--billing-address")]
    public string? BillingAddress { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--party-type-string")]
    public string? PartyTypeString { get; set; }

    [CommandSwitch("--gender-string")]
    public string? GenderString { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}