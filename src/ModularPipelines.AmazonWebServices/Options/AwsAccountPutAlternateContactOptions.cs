using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "put-alternate-contact")]
public record AwsAccountPutAlternateContactOptions(
[property: CommandSwitch("--alternate-contact-type")] string AlternateContactType,
[property: CommandSwitch("--email-address")] string EmailAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--phone-number")] string PhoneNumber,
[property: CommandSwitch("--title")] string Title
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}