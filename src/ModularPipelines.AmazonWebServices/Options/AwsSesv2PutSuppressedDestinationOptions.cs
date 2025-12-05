using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-suppressed-destination")]
public record AwsSesv2PutSuppressedDestinationOptions(
[property: CliOption("--email-address")] string EmailAddress,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}