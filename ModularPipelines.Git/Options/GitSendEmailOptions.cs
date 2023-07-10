using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("send-email")]
public record GitSendEmailOptions : GitOptions
{
    [BooleanCommandSwitch("annotate")]
    public bool? Annotate { get; set; }

    [CommandLongSwitch("bcc")]
    public string? Bcc { get; set; }

    [CommandLongSwitch("cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("compose")]
    public bool? Compose { get; set; }

    [CommandLongSwitch("from")]
    public string? From { get; set; }

    [CommandLongSwitch("reply-to")]
    public string? ReplyTo { get; set; }

    [CommandLongSwitch("in-reply-to")]
    public string? InReplyTo { get; set; }

    [CommandLongSwitch("subject")]
    public string? Subject { get; set; }

    [CommandLongSwitch("to")]
    public string? To { get; set; }

    [CommandLongSwitch("8bit-encoding")]
    public string? EightBitEncoding { get; set; }

    [CommandLongSwitch("compose-encoding")]
    public string? ComposeEncoding { get; set; }

    [BooleanCommandSwitch("transfer-encoding")]
    public bool? TransferEncoding { get; set; }

    [BooleanCommandSwitch("xmailer")]
    public bool? Xmailer { get; set; }

    [BooleanCommandSwitch("no-xmailer")]
    public bool? NoXmailer { get; set; }

    [CommandLongSwitch("envelope-sender")]
    public string? EnvelopeSender { get; set; }

    [CommandLongSwitch("sendmail-cmd")]
    public string? SendmailCmd { get; set; }

    [CommandLongSwitch("smtp-encryption")]
    public string? SmtpEncryption { get; set; }

    [CommandLongSwitch("smtp-domain")]
    public string? SmtpDomain { get; set; }

    [CommandLongSwitch("smtp-auth")]
    public string? SmtpAuth { get; set; }

    [CommandLongSwitch("smtp-pass")]
    public string? SmtpPass { get; set; }

    [BooleanCommandSwitch("no-smtp-auth")]
    public bool? NoSmtpAuth { get; set; }

    [CommandLongSwitch("smtp-server")]
    public string? SmtpServer { get; set; }

    [CommandLongSwitch("smtp-server-port")]
    public string? SmtpServerPort { get; set; }

    [CommandLongSwitch("smtp-server-option")]
    public string? SmtpServerOption { get; set; }

    [BooleanCommandSwitch("smtp-ssl")]
    public bool? SmtpSsl { get; set; }

    [BooleanCommandSwitch("smtp-ssl-cert-path")]
    public bool? SmtpSslCertPath { get; set; }

    [CommandLongSwitch("smtp-user")]
    public string? SmtpUser { get; set; }

    [BooleanCommandSwitch("smtp-debug")]
    public bool? SmtpDebug { get; set; }

    [CommandLongSwitch("batch-size")]
    public string? BatchSize { get; set; }

    [CommandLongSwitch("relogin-delay")]
    public string? ReloginDelay { get; set; }

    [BooleanCommandSwitch("no-to")]
    public bool? NoTo { get; set; }

    [BooleanCommandSwitch("no-cc")]
    public bool? NoCc { get; set; }

    [BooleanCommandSwitch("no-bcc")]
    public bool? NoBcc { get; set; }

    [BooleanCommandSwitch("no-identity")]
    public bool? NoIdentity { get; set; }

    [CommandLongSwitch("to-cmd")]
    public string? ToCmd { get; set; }

    [CommandLongSwitch("cc-cmd")]
    public string? CcCmd { get; set; }

    [CommandLongSwitch("header-cmd")]
    public string? HeaderCmd { get; set; }

    [BooleanCommandSwitch("no-header-cmd")]
    public bool? NoHeaderCmd { get; set; }

    [BooleanCommandSwitch("no-chain-reply-to")]
    public bool? NoChainReplyTo { get; set; }

    [BooleanCommandSwitch("chain-reply-to")]
    public bool? ChainReplyTo { get; set; }

    [CommandLongSwitch("identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("no-signed-off-by-cc")]
    public bool? NoSignedOffByCc { get; set; }

    [BooleanCommandSwitch("signed-off-by-cc")]
    public bool? SignedOffByCc { get; set; }

    [BooleanCommandSwitch("no-cc-cover")]
    public bool? NoCcCover { get; set; }

    [BooleanCommandSwitch("cc-cover")]
    public bool? CcCover { get; set; }

    [BooleanCommandSwitch("no-to-cover")]
    public bool? NoToCover { get; set; }

    [BooleanCommandSwitch("to-cover")]
    public bool? ToCover { get; set; }

    [CommandLongSwitch("suppress-cc")]
    public string? SuppressCc { get; set; }

    [BooleanCommandSwitch("no-suppress-from")]
    public bool? NoSuppressFrom { get; set; }

    [BooleanCommandSwitch("suppress-from")]
    public bool? SuppressFrom { get; set; }

    [BooleanCommandSwitch("no-thread")]
    public bool? NoThread { get; set; }

    [BooleanCommandSwitch("thread")]
    public bool? Thread { get; set; }

    [CommandLongSwitch("confirm")]
    public string? Confirm { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("no-format-patch")]
    public bool? NoFormatPatch { get; set; }

    [BooleanCommandSwitch("format-patch")]
    public bool? FormatPatch { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("no-validate")]
    public bool? NoValidate { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("dump-aliases")]
    public bool? DumpAliases { get; set; }

}