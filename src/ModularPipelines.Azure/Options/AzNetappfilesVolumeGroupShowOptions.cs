using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume-group", "show")]
public record AzNetappfilesVolumeGroupShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;