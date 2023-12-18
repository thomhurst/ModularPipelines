using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "update")]
public record AzBotUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ai-api-key")]
    public string? AiApiKey { get; set; }

    [CommandSwitch("--ai-app-id")]
    public string? AiAppId { get; set; }

    [CommandSwitch("--ai-key")]
    public string? AiKey { get; set; }

    [CommandSwitch("--cmk")]
    public string? Cmk { get; set; }

    [CommandSwitch("--cmk-off")]
    public string? CmkOff { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--icon-url")]
    public string? IconUrl { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}