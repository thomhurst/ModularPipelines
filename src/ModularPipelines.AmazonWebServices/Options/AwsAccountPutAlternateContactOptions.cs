using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "put-alternate-contact")]
public record AwsAccountPutAlternateContactOptions(
[property: CliOption("--alternate-contact-type")] string AlternateContactType,
[property: CliOption("--email-address")] string EmailAddress,
[property: CliOption("--name")] string Name,
[property: CliOption("--phone-number")] string PhoneNumber,
[property: CliOption("--title")] string Title
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}