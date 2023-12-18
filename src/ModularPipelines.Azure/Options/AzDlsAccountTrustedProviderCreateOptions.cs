using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "account", "trusted-provider", "create")]
public record AzDlsAccountTrustedProviderCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--id-provider")] string IdProvider,
[property: CommandSwitch("--trusted-id-provider-name")] string TrustedIdProviderName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}