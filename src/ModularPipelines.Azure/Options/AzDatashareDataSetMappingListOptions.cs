using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "data-set-mapping", "list")]
public record AzDatashareDataSetMappingListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}