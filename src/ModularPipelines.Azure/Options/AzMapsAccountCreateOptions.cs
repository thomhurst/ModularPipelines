using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maps", "account", "create")]
public record AzMapsAccountCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--accept-tos")]
    public string? AcceptTos { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--linked-resources")]
    public string? LinkedResources { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-identities")]
    public string? UserIdentities { get; set; }
}