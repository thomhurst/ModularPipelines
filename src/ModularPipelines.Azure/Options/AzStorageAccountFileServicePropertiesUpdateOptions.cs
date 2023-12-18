using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "file-service-properties", "update")]
public record AzStorageAccountFileServicePropertiesUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auth-methods")]
    public string? AuthMethods { get; set; }

    [CommandSwitch("--channel-encryption")]
    public string? ChannelEncryption { get; set; }

    [CommandSwitch("--delete-retention-days")]
    public string? DeleteRetentionDays { get; set; }

    [BooleanCommandSwitch("--enable-delete-retention")]
    public bool? EnableDeleteRetention { get; set; }

    [BooleanCommandSwitch("--enable-smb-multichannel")]
    public bool? EnableSmbMultichannel { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--kerb-ticket-encryption")]
    public string? KerbTicketEncryption { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--versions")]
    public string? Versions { get; set; }
}