using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("send-email")]
[ExcludeFromCodeCoverage]
public record GitSendEmailOptions : GitOptions
{
    [BooleanCommandSwitch("--annotate")]
    public virtual bool? Annotate { get; set; }

    [CommandEqualsSeparatorSwitch("--bcc")]
    public string? Bcc { get; set; }

    [CommandEqualsSeparatorSwitch("--cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("--compose")]
    public virtual bool? Compose { get; set; }

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
    public virtual bool? TransferEncoding { get; set; }

    [BooleanCommandSwitch("--xmailer")]
    public virtual bool? Xmailer { get; set; }

    [BooleanCommandSwitch("--no-xmailer")]
    public virtual bool? NoXmailer { get; set; }

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
    public virtual bool? NoSmtpAuth { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server")]
    public string? SmtpServer { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server-port")]
    public string? SmtpServerPort { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-server-option")]
    public string? SmtpServerOption { get; set; }

    [BooleanCommandSwitch("--smtp-ssl")]
    public virtual bool? SmtpSsl { get; set; }

    [BooleanCommandSwitch("--smtp-ssl-cert-path")]
    public virtual bool? SmtpSslCertPath { get; set; }

    [CommandEqualsSeparatorSwitch("--smtp-user")]
    public string? SmtpUser { get; set; }

    [BooleanCommandSwitch("--smtp-debug")]
    public virtual bool? SmtpDebug { get; set; }

    [CommandEqualsSeparatorSwitch("--batch-size")]
    public string? BatchSize { get; set; }

    [CommandEqualsSeparatorSwitch("--relogin-delay")]
    public string? ReloginDelay { get; set; }

    [BooleanCommandSwitch("--no-to")]
    public virtual bool? NoTo { get; set; }

    [BooleanCommandSwitch("--no-cc")]
    public virtual bool? NoCc { get; set; }

    [BooleanCommandSwitch("--no-bcc")]
    public virtual bool? NoBcc { get; set; }

    [BooleanCommandSwitch("--no-identity")]
    public virtual bool? NoIdentity { get; set; }

    [CommandEqualsSeparatorSwitch("--to-cmd")]
    public string? ToCmd { get; set; }

    [CommandEqualsSeparatorSwitch("--cc-cmd")]
    public string? CcCmd { get; set; }

    [CommandEqualsSeparatorSwitch("--header-cmd")]
    public string? HeaderCmd { get; set; }

    [BooleanCommandSwitch("--no-header-cmd")]
    public virtual bool? NoHeaderCmd { get; set; }

    [BooleanCommandSwitch("--no-chain-reply-to")]
    public virtual bool? NoChainReplyTo { get; set; }

    [BooleanCommandSwitch("--chain-reply-to")]
    public virtual bool? ChainReplyTo { get; set; }

    [CommandEqualsSeparatorSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--no-signed-off-by-cc")]
    public virtual bool? NoSignedOffByCc { get; set; }

    [BooleanCommandSwitch("--signed-off-by-cc")]
    public virtual bool? SignedOffByCc { get; set; }

    [BooleanCommandSwitch("--no-cc-cover")]
    public virtual bool? NoCcCover { get; set; }

    [BooleanCommandSwitch("--cc-cover")]
    public virtual bool? CcCover { get; set; }

    [BooleanCommandSwitch("--no-to-cover")]
    public virtual bool? NoToCover { get; set; }

    [BooleanCommandSwitch("--to-cover")]
    public virtual bool? ToCover { get; set; }

    [CommandEqualsSeparatorSwitch("--suppress-cc")]
    public string? SuppressCc { get; set; }

    [BooleanCommandSwitch("--no-suppress-from")]
    public virtual bool? NoSuppressFrom { get; set; }

    [BooleanCommandSwitch("--suppress-from")]
    public virtual bool? SuppressFrom { get; set; }

    [BooleanCommandSwitch("--no-thread")]
    public virtual bool? NoThread { get; set; }

    [BooleanCommandSwitch("--thread")]
    public virtual bool? Thread { get; set; }

    [CommandEqualsSeparatorSwitch("--confirm")]
    public string? Confirm { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--no-format-patch")]
    public virtual bool? NoFormatPatch { get; set; }

    [BooleanCommandSwitch("--format-patch")]
    public virtual bool? FormatPatch { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--no-validate")]
    public virtual bool? NoValidate { get; set; }

    [BooleanCommandSwitch("--validate")]
    public virtual bool? Validate { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--dump-aliases")]
    public virtual bool? DumpAliases { get; set; }
}