using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "account", "trusted-provider", "create")]
public record AzDlsAccountTrustedProviderCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--id-provider")] string IdProvider,
[property: CliOption("--trusted-id-provider-name")] string TrustedIdProviderName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}