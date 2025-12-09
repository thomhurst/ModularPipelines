using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maps", "account", "update")]
public record AzMapsAccountUpdateOptions(
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--linked-resources")]
    public string? LinkedResources { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-identities")]
    public string? UserIdentities { get; set; }
}