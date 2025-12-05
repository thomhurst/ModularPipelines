using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-email-identity-dkim-attributes")]
public record AwsSesv2PutEmailIdentityDkimAttributesOptions(
[property: CliOption("--email-identity")] string EmailIdentity
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}