using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "synchronization-setting", "create")]
public record AzDatashareSynchronizationSettingCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions
{
    [CommandSwitch("--scheduled-synchronization-setting")]
    public string? ScheduledSynchronizationSetting { get; set; }
}