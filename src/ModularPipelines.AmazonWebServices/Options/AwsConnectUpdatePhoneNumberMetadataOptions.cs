using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-phone-number-metadata")]
public record AwsConnectUpdatePhoneNumberMetadataOptions(
[property: CliOption("--phone-number-id")] string PhoneNumberId
) : AwsOptions
{
    [CliOption("--phone-number-description")]
    public string? PhoneNumberDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}