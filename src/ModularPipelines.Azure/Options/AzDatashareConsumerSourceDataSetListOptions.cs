using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "consumer-source-data-set", "list")]
public record AzDatashareConsumerSourceDataSetListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}