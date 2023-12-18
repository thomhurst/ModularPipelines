using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "replica", "create")]
public record AzAppconfigReplicaCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--store-name")] string StoreName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}