using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "authsetting", "create")]
public record AzBotAuthsettingCreateOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--client-secret")] string ClientSecret,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-scope-string")] string ProviderScopeString,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--setting-name")] string SettingName
) : AzOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }
}