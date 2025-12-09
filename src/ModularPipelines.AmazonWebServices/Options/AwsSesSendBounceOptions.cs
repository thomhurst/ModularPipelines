using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-bounce")]
public record AwsSesSendBounceOptions(
[property: CliOption("--original-message-id")] string OriginalMessageId,
[property: CliOption("--bounce-sender")] string BounceSender,
[property: CliOption("--bounced-recipient-info-list")] string[] BouncedRecipientInfoList
) : AwsOptions
{
    [CliOption("--explanation")]
    public string? Explanation { get; set; }

    [CliOption("--message-dsn")]
    public string? MessageDsn { get; set; }

    [CliOption("--bounce-sender-arn")]
    public string? BounceSenderArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}