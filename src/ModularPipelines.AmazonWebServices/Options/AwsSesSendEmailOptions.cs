using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-email")]
public record AwsSesSendEmailOptions(
[property: CliOption("--from")] string From
) : AwsOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--return-path")]
    public string? ReturnPath { get; set; }

    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--bcc")]
    public string? Bcc { get; set; }

    [CliOption("--subject")]
    public string? Subject { get; set; }

    [CliOption("--text")]
    public string? Text { get; set; }

    [CliOption("--html")]
    public string? Html { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}