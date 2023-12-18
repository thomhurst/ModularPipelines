using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("restore-point", "collection", "create")]
public record AzRestorePointCollectionCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-id")] string SourceId
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

