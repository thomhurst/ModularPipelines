using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "email", "send")]
public record AzCommunicationEmailSendOptions(
[property: CommandSwitch("--sender")] string Sender,
[property: CommandSwitch("--subject")] string Subject
) : AzOptions
{
    [BooleanCommandSwitch("--attachment-types")]
    public bool? AttachmentTypes { get; set; }

    [BooleanCommandSwitch("--attachments")]
    public bool? Attachments { get; set; }

    [CommandSwitch("--bcc")]
    public string? Bcc { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--disable-tracking")]
    public bool? DisableTracking { get; set; }

    [CommandSwitch("--html")]
    public string? Html { get; set; }

    [CommandSwitch("--importance")]
    public string? Importance { get; set; }

    [CommandSwitch("--reply-to")]
    public string? ReplyTo { get; set; }

    [CommandSwitch("--text")]
    public string? Text { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }
}