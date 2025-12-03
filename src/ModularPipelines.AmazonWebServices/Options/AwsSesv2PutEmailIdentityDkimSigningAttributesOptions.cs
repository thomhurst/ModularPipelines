using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-email-identity-dkim-signing-attributes")]
public record AwsSesv2PutEmailIdentityDkimSigningAttributesOptions(
[property: CliOption("--email-identity")] string EmailIdentity,
[property: CliOption("--signing-attributes-origin")] string SigningAttributesOrigin,
[property: CliOption("--signing-attributes")] string SigningAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}