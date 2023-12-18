using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "provider-share-subscription", "list")]
public record AzDatashareProviderShareSubscriptionListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}