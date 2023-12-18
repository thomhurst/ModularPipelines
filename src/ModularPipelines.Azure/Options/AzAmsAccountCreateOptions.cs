using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account", "create")]
public record AzAmsAccountCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [BooleanCommandSwitch("--disable-public-network")]
    public bool? DisablePublicNetwork { get; set; }

    [CommandSwitch("--ip-allow-list")]
    public string? IpAllowList { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}