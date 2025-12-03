using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "file-service-properties", "update")]
public record AzStorageAccountFileServicePropertiesUpdateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auth-methods")]
    public string? AuthMethods { get; set; }

    [CliOption("--channel-encryption")]
    public string? ChannelEncryption { get; set; }

    [CliOption("--delete-retention-days")]
    public string? DeleteRetentionDays { get; set; }

    [CliFlag("--enable-delete-retention")]
    public bool? EnableDeleteRetention { get; set; }

    [CliFlag("--enable-smb-multichannel")]
    public bool? EnableSmbMultichannel { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--kerb-ticket-encryption")]
    public string? KerbTicketEncryption { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--versions")]
    public string? Versions { get; set; }
}