using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "share-rm", "list")]
public record AzStorageShareRmListOptions(
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--include-deleted")]
    public string? IncludeDeleted { get; set; }

    [CommandSwitch("--include-snapshot")]
    public string? IncludeSnapshot { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}