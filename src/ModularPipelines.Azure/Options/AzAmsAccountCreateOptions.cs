using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "account", "create")]
public record AzAmsAccountCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--disable-public-network")]
    public bool? DisablePublicNetwork { get; set; }

    [CliOption("--ip-allow-list")]
    public string? IpAllowList { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}