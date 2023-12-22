using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "atp", "storage", "update")]
public record AzSecurityAtpStorageUpdateOptions(
[property: BooleanCommandSwitch("--is-enabled")] bool IsEnabled,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions;