using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "describe-opted-out-numbers")]
public record AwsPinpointSmsVoiceV2DescribeOptedOutNumbersOptions(
[property: CliOption("--opt-out-list-name")] string OptOutListName
) : AwsOptions
{
    [CliOption("--opted-out-numbers")]
    public string[]? OptedOutNumbers { get; set; }

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