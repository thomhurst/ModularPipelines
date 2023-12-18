using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "binding", "redis", "add")]
public record AzSpringAppBindingRedisAddOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--disable-ssl")]
    public bool? DisableSsl { get; set; }
}

