using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "validate-e911-address")]
public record AwsChimeSdkVoiceValidateE911AddressOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--street-number")] string StreetNumber,
[property: CliOption("--street-info")] string StreetInfo,
[property: CliOption("--city")] string City,
[property: CliOption("--state")] string State,
[property: CliOption("--country")] string Country,
[property: CliOption("--postal-code")] string PostalCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}