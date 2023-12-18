using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "binding", "cosmos", "add")]
public record AzSpringCloudAppBindingCosmosAddOptions(
[property: CommandSwitch("--api-type")] string ApiType,
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--collection-name")]
    public string? CollectionName { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--key-space")]
    public string? KeySpace { get; set; }
}

