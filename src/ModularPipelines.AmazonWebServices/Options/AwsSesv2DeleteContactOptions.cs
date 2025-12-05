using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "delete-contact")]
public record AwsSesv2DeleteContactOptions(
[property: CliOption("--contact-list-name")] string ContactListName,
[property: CliOption("--email-address")] string EmailAddress
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}