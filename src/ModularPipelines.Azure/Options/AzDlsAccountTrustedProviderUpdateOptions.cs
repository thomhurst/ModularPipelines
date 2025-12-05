using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "account", "trusted-provider", "update")]
public record AzDlsAccountTrustedProviderUpdateOptions(
[property: CliOption("--trusted-id-provider-name")] string TrustedIdProviderName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--id-provider")]
    public string? IdProvider { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}