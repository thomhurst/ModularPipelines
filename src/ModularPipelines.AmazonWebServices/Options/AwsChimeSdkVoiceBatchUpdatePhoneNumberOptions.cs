using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "batch-update-phone-number")]
public record AwsChimeSdkVoiceBatchUpdatePhoneNumberOptions(
[property: CliOption("--update-phone-number-request-items")] string[] UpdatePhoneNumberRequestItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}