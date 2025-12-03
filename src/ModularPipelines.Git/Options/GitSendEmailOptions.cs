using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("send-email")]
[ExcludeFromCodeCoverage]
public record GitSendEmailOptions : GitOptions
{
    [CliFlag("--annotate")]
    public virtual bool? Annotate { get; set; }

    [CliOption("--bcc", Format = OptionFormat.EqualsSeparated)]
    public string? Bcc { get; set; }

    [CliOption("--cc", Format = OptionFormat.EqualsSeparated)]
    public string? Cc { get; set; }

    [CliFlag("--compose")]
    public virtual bool? Compose { get; set; }

    [CliOption("--from", Format = OptionFormat.EqualsSeparated)]
    public string? From { get; set; }

    [CliOption("--reply-to", Format = OptionFormat.EqualsSeparated)]
    public string? ReplyTo { get; set; }

    [CliOption("--in-reply-to", Format = OptionFormat.EqualsSeparated)]
    public string? InReplyTo { get; set; }

    [CliOption("--subject", Format = OptionFormat.EqualsSeparated)]
    public string? Subject { get; set; }

    [CliOption("--to", Format = OptionFormat.EqualsSeparated)]
    public string? To { get; set; }

    [CliOption("--8bit-encoding", Format = OptionFormat.EqualsSeparated)]
    public string? EightBitEncoding { get; set; }

    [CliOption("--compose-encoding", Format = OptionFormat.EqualsSeparated)]
    public string? ComposeEncoding { get; set; }

    [CliFlag("--transfer-encoding")]
    public virtual bool? TransferEncoding { get; set; }

    [CliFlag("--xmailer")]
    public virtual bool? Xmailer { get; set; }

    [CliFlag("--no-xmailer")]
    public virtual bool? NoXmailer { get; set; }

    [CliOption("--envelope-sender", Format = OptionFormat.EqualsSeparated)]
    public string? EnvelopeSender { get; set; }

    [CliOption("--sendmail-cmd", Format = OptionFormat.EqualsSeparated)]
    public string? SendmailCmd { get; set; }

    [CliOption("--smtp-encryption", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpEncryption { get; set; }

    [CliOption("--smtp-domain", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpDomain { get; set; }

    [CliOption("--smtp-auth", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpAuth { get; set; }

    [CliOption("--smtp-pass", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpPass { get; set; }

    [CliFlag("--no-smtp-auth")]
    public virtual bool? NoSmtpAuth { get; set; }

    [CliOption("--smtp-server", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpServer { get; set; }

    [CliOption("--smtp-server-port", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpServerPort { get; set; }

    [CliOption("--smtp-server-option", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpServerOption { get; set; }

    [CliFlag("--smtp-ssl")]
    public virtual bool? SmtpSsl { get; set; }

    [CliFlag("--smtp-ssl-cert-path")]
    public virtual bool? SmtpSslCertPath { get; set; }

    [CliOption("--smtp-user", Format = OptionFormat.EqualsSeparated)]
    public string? SmtpUser { get; set; }

    [CliFlag("--smtp-debug")]
    public virtual bool? SmtpDebug { get; set; }

    [CliOption("--batch-size", Format = OptionFormat.EqualsSeparated)]
    public string? BatchSize { get; set; }

    [CliOption("--relogin-delay", Format = OptionFormat.EqualsSeparated)]
    public string? ReloginDelay { get; set; }

    [CliFlag("--no-to")]
    public virtual bool? NoTo { get; set; }

    [CliFlag("--no-cc")]
    public virtual bool? NoCc { get; set; }

    [CliFlag("--no-bcc")]
    public virtual bool? NoBcc { get; set; }

    [CliFlag("--no-identity")]
    public virtual bool? NoIdentity { get; set; }

    [CliOption("--to-cmd", Format = OptionFormat.EqualsSeparated)]
    public string? ToCmd { get; set; }

    [CliOption("--cc-cmd", Format = OptionFormat.EqualsSeparated)]
    public string? CcCmd { get; set; }

    [CliOption("--header-cmd", Format = OptionFormat.EqualsSeparated)]
    public string? HeaderCmd { get; set; }

    [CliFlag("--no-header-cmd")]
    public virtual bool? NoHeaderCmd { get; set; }

    [CliFlag("--no-chain-reply-to")]
    public virtual bool? NoChainReplyTo { get; set; }

    [CliFlag("--chain-reply-to")]
    public virtual bool? ChainReplyTo { get; set; }

    [CliOption("--identity", Format = OptionFormat.EqualsSeparated)]
    public string? Identity { get; set; }

    [CliFlag("--no-signed-off-by-cc")]
    public virtual bool? NoSignedOffByCc { get; set; }

    [CliFlag("--signed-off-by-cc")]
    public virtual bool? SignedOffByCc { get; set; }

    [CliFlag("--no-cc-cover")]
    public virtual bool? NoCcCover { get; set; }

    [CliFlag("--cc-cover")]
    public virtual bool? CcCover { get; set; }

    [CliFlag("--no-to-cover")]
    public virtual bool? NoToCover { get; set; }

    [CliFlag("--to-cover")]
    public virtual bool? ToCover { get; set; }

    [CliOption("--suppress-cc", Format = OptionFormat.EqualsSeparated)]
    public string? SuppressCc { get; set; }

    [CliFlag("--no-suppress-from")]
    public virtual bool? NoSuppressFrom { get; set; }

    [CliFlag("--suppress-from")]
    public virtual bool? SuppressFrom { get; set; }

    [CliFlag("--no-thread")]
    public virtual bool? NoThread { get; set; }

    [CliFlag("--thread")]
    public virtual bool? Thread { get; set; }

    [CliOption("--confirm", Format = OptionFormat.EqualsSeparated)]
    public string? Confirm { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--no-format-patch")]
    public virtual bool? NoFormatPatch { get; set; }

    [CliFlag("--format-patch")]
    public virtual bool? FormatPatch { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--no-validate")]
    public virtual bool? NoValidate { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--dump-aliases")]
    public virtual bool? DumpAliases { get; set; }
}