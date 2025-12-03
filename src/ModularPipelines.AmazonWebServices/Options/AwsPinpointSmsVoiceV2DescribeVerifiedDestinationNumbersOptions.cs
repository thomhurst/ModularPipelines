using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "describe-verified-destination-numbers")]
public record AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions : AwsOptions
{
    [CliOption("--verified-destination-number-ids")]
    public string[]? VerifiedDestinationNumberIds { get; set; }

    [CliOption("--destination-phone-numbers")]
    public string[]? DestinationPhoneNumbers { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}