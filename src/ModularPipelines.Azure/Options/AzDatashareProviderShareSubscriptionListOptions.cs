using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "provider-share-subscription", "list")]
public record AzDatashareProviderShareSubscriptionListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-name")] string ShareName
) : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}