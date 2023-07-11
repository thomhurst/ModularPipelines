using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("send-email")]
public record GitSendEmailOptions : GitOptions
{
    [BooleanCommandSwitch("--annotate")]
    public bool? Annotate { get; set; }

    [CommandEqualsSeparatorSwitch("--bcc")]
    public string? Bcc { get; set; }

    [CommandEqualsSeparatorSwitch("--cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("--compose")]
    public bool? Compose { get; set; }

    [CommandEqualsSeparatorSwitch("--from")]
    public string? From { get; set; }

    [CommandEqualsSeparatorSwitch("--reply-to")]
    public string? ReplyTo { get; set; }

    [CommandEqualsSeparatorSwitch("--in-reply-to")]
    public string? InReplyTo { get; set; }

    [CommandEqualsSeparatorSwitch("--subject")]
    public string? Subject { get; set; }

    [CommandEqualsSeparatorSwitch("--to")]
    public string? To { get; set; }

    [CommandEqualsSeparatorSwitch("--8bit-encoding")]
    public string? EightBitEncoding { get; set; }

    [CommandEqualsSeparatorSwitch("--compose-encoding")]
    public string? ComposeEncoding { get; set; }

    [BooleanCommandSwitch("--transfer-encoding")]
    public bool? TransferEncoding { get; set; }

    [BooleanCommandSwitch("--xmailer")]
    public bool? Xmailer { get; set; }

    [BooleanCommandSwitch("--no-xmailer")]
    public bool? NoXmailer { get; set; }

    [CommandEqualsSeparatorSwitch("--envelope-sender")]
    public string? EnvelopeSender { get; set; }

    [CommandEqualsSeparatorSwitch("--sendmail-cmd")]
    public string? SendmailCmd { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-encryption")]
    public string? SmtpEncryption { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-domain")]
    public string? SmtpDomain { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-auth")]
    public string? SmtpAuth { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-pass")]
    public string? SmtpPass { get; set; }

    [BooleanCommandSwitch("--no-smtp-auth")]
    public bool? NoSmtpAuth { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server")]
    public string? SmtpServer { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server-port")]
    public string? SmtpServerPort { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server-option")]
    public string? SmtpServerOption { get; set; }

    [BooleanCommandSwitch("--smtp-ssl")]
    public bool? SmtpSsl { get; set; }

    [BooleanCommandSwitch("--smtp-ssl-cert-path")]
    public bool? SmtpSslCertPath { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-user")]
    public string? SmtpUser { get; set; }

    [BooleanCommandSwitch("--smtp-debug")]
    public bool? SmtpDebug { get; set; }

    [CommandEqualsSeparatorSwitch("--batch-size")]
    public string? BatchSize { get; set; }

    [CommandEqualsSeparatorSwitch("--relogin-delay")]
    public string? ReloginDelay { get; set; }

    [BooleanCommandSwitch("--no-to")]
    public bool? NoTo { get; set; }

    [BooleanCommandSwitch("--no-cc")]
    public bool? NoCc { get; set; }

    [BooleanCommandSwitch("--no-bcc")]
    public bool? NoBcc { get; set; }

    [BooleanCommandSwitch("--no-identity")]
    public bool? NoIdentity { get; set; }

    [CommandEqualsSeparatorSwitch("--to-cmd")]
    public string? ToCmd { get; set; }

    [CommandEqualsSeparatorSwitch("--cc-cmd")]
    public string? CcCmd { get; set; }

    [CommandEqualsSeparatorSwitch("--header-cmd")]
    public string? HeaderCmd { get; set; }

    [BooleanCommandSwitch("--no-header-cmd")]
    public bool? NoHeaderCmd { get; set; }

    [BooleanCommandSwitch("--no-chain-reply-to")]
    public bool? NoChainReplyTo { get; set; }

    [BooleanCommandSwitch("--chain-reply-to")]
    public bool? ChainReplyTo { get; set; }

    [CommandEqualsSeparatorSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--no-signed-off-by-cc")]
    public bool? NoSignedOffByCc { get; set; }

    [BooleanCommandSwitch("--signed-off-by-cc")]
    public bool? SignedOffByCc { get; set; }

    [BooleanCommandSwitch("--no-cc-cover")]
    public bool? NoCcCover { get; set; }

    [BooleanCommandSwitch("--cc-cover")]
    public bool? CcCover { get; set; }

    [BooleanCommandSwitch("--no-to-cover")]
    public bool? NoToCover { get; set; }

    [BooleanCommandSwitch("--to-cover")]
    public bool? ToCover { get; set; }

    [CommandEqualsSeparatorSwitch("--suppress-cc")]
    public string? SuppressCc { get; set; }

    [BooleanCommandSwitch("--no-suppress-from")]
    public bool? NoSuppressFrom { get; set; }

    [BooleanCommandSwitch("--suppress-from")]
    public bool? SuppressFrom { get; set; }

    [BooleanCommandSwitch("--no-thread")]
    public bool? NoThread { get; set; }

    [BooleanCommandSwitch("--thread")]
    public bool? Thread { get; set; }

    [CommandEqualsSeparatorSwitch("--confirm")]
    public string? Confirm { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--no-format-patch")]
    public bool? NoFormatPatch { get; set; }

    [BooleanCommandSwitch("--format-patch")]
    public bool? FormatPatch { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--no-validate")]
    public bool? NoValidate { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--dump-aliases")]
    public bool? DumpAliases { get; set; }

}