using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "share-subscription", "list-source-share-synchronization-setting")]
public record AzDatashareShareSubscriptionListSourceShareSynchronizationSettingOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}