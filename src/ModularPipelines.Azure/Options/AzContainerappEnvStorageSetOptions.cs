using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "storage", "set")]
public record AzContainerappEnvStorageSetOptions(
[property: CommandSwitch("--access-mode")] string AccessMode,
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--azure-file-account-key")] int AzureFileAccountKey,
[property: CommandSwitch("--azure-file-share-name")] string AzureFileShareName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-name")] string StorageName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}