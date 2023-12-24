using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "send-bounce")]
public record AwsSesSendBounceOptions(
[property: CommandSwitch("--original-message-id")] string OriginalMessageId,
[property: CommandSwitch("--bounce-sender")] string BounceSender,
[property: CommandSwitch("--bounced-recipient-info-list")] string[] BouncedRecipientInfoList
) : AwsOptions
{
    [CommandSwitch("--explanation")]
    public string? Explanation { get; set; }

    [CommandSwitch("--message-dsn")]
    public string? MessageDsn { get; set; }

    [CommandSwitch("--bounce-sender-arn")]
    public string? BounceSenderArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}