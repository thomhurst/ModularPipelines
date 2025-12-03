using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "email", "send")]
public record AzCommunicationEmailSendOptions(
[property: CliOption("--sender")] string Sender,
[property: CliOption("--subject")] string Subject
) : AzOptions
{
    [CliFlag("--attachment-types")]
    public bool? AttachmentTypes { get; set; }

    [CliFlag("--attachments")]
    public bool? Attachments { get; set; }

    [CliOption("--bcc")]
    public string? Bcc { get; set; }

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--disable-tracking")]
    public bool? DisableTracking { get; set; }

    [CliOption("--html")]
    public string? Html { get; set; }

    [CliOption("--importance")]
    public string? Importance { get; set; }

    [CliOption("--reply-to")]
    public string? ReplyTo { get; set; }

    [CliOption("--text")]
    public string? Text { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }
}