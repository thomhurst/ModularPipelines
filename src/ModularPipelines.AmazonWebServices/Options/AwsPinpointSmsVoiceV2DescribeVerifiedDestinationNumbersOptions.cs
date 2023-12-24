using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "describe-verified-destination-numbers")]
public record AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions : AwsOptions
{
    [CommandSwitch("--verified-destination-number-ids")]
    public string[]? VerifiedDestinationNumberIds { get; set; }

    [CommandSwitch("--destination-phone-numbers")]
    public string[]? DestinationPhoneNumbers { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}