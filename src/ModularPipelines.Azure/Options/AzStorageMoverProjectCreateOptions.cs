using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "project", "create")]
public record AzStorageMoverProjectCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}

