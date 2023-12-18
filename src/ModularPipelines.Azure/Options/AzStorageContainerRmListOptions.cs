using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container-rm", "list")]
public record AzStorageContainerRmListOptions(
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--include-deleted")]
    public string? IncludeDeleted { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}