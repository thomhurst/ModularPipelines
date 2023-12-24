using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "get-contact")]
public record AwsSesv2GetContactOptions(
[property: CommandSwitch("--contact-list-name")] string ContactListName,
[property: CommandSwitch("--email-address")] string EmailAddress
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}