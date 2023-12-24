using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "send-email")]
public record AwsSesSendEmailOptions(
[property: CommandSwitch("--from")] string From
) : AwsOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--return-path")]
    public string? ReturnPath { get; set; }

    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--bcc")]
    public string? Bcc { get; set; }

    [CommandSwitch("--subject")]
    public string? Subject { get; set; }

    [CommandSwitch("--text")]
    public string? Text { get; set; }

    [CommandSwitch("--html")]
    public string? Html { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}